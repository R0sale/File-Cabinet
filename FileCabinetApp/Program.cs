using FileCabinetApp;
using FileCabinetApp.CommandHandlers;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Markup;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Kirill Vusov";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
#pragma warning disable CA1859
        public static IFileCabinetService? fileCabinetService;
#pragma warning restore CA1859
        public static string typeOfTheRules = "default";

        public static bool isRunning = true;

        private static string[] commands = new string[]
        {
            "help",
            "exit",
            "stat",
            "create",
            "list",
            "edit",
            "find",
            "export",
            "import",
            "remove",
            "purge",
        };

        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="args">Just arguments.</param>
        public static void Main(string[] args)
        {
            if (args != null)
            {
                if (args.Length > 0)
                {
                    if (args.Contains("-v"))
                    {
                        int index = Array.IndexOf(args, "-v");

                        if (args[index + 1].Equals("custom", StringComparison.OrdinalIgnoreCase))
                        {
                            typeOfTheRules = "custom";
                        }
                    }
                    else if (args.Contains("--validation-rules=custom"))
                    {
                        typeOfTheRules = "custom";
                    }

                    if (args.Contains("--storage"))
                    {
                        int index = Array.IndexOf(args, "--storage");

                        if (args[index + 1].Equals("file", StringComparison.OrdinalIgnoreCase))
                        {
                            fileCabinetService = new FileCabinetFilesystemService("cabinet - records.db");
                        }
                        else if (args[index + 1].Equals("memory", StringComparison.OrdinalIgnoreCase))
                        {
                            if (typeOfTheRules.Equals("custom", StringComparison.OrdinalIgnoreCase))
                            {
                                fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                            }
                            else
                            {
                                fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                            }
                        }
                    }
                    else if (args.Contains("-s"))
                    {
                        int index = Array.IndexOf(args, "-s");

                        if (args[index + 1].Equals("file", StringComparison.OrdinalIgnoreCase))
                        {
                            fileCabinetService = new FileCabinetFilesystemService("cabinet - records.db");
                        }
                        else if (args[index + 1].Equals("memory", StringComparison.OrdinalIgnoreCase))
                        {
                            if (typeOfTheRules.Equals("custom", StringComparison.OrdinalIgnoreCase))
                            {
                                fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                            }
                            else
                            {
                                fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                            }
                        }
                    }
                    else
                    {
                        fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                    }
                }
                else
                {
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                }
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine($"Using {typeOfTheRules} validation rules.");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    AppCommandRequest request = new AppCommandRequest(command, parameters);
                    var commandHandler = CreateCommandHandlers();
                    commandHandler.Handle(request);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler();
            var editHandler = new EditCommandHandler();
            var listHandler = new ListCommandHandler();
            var importHandler = new ImportCommandHandler();
            var exportHandler = new ExportCommandHandler();
            var findHandler = new FindCommandHandler();
            var statHandler = new StatCommandHandler();
            var removeHandler = new RemoveCommandHandler();
            var purgeHandler = new PurgeCommandHandler();
            var exitHandler = new ExitCommandHandler();

            helpHandler.SetNext(createHandler);
            createHandler.SetNext(editHandler);
            editHandler.SetNext(listHandler);
            listHandler.SetNext(importHandler);
            importHandler.SetNext(exportHandler);
            exportHandler.SetNext(findHandler);
            findHandler.SetNext(statHandler);
            statHandler.SetNext(removeHandler);
            removeHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(exitHandler);

            return helpHandler;
        }

        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                if (input == null)
                {
                    throw new ArgumentException("The input is null");
                }

                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        public static Func<string, Tuple<bool, string, string>> stringConverter = (string str) =>
        {
            return new Tuple<bool, string, string>(!string.IsNullOrEmpty(str), "String is null or empty", str);
        };

        public static Func<string, Tuple<bool, string, DateTime>> dateConverter = (string str) =>
        {
            DateTime dateOfBirth;
            return new Tuple<bool, string, DateTime>(DateTime.TryParseExact(str, "MM/dd/yyyy", null, DateTimeStyles.None, out dateOfBirth), "String is null or empty", dateOfBirth);
        };

        public static Func<string, Tuple<bool, string, char>> charConverter = (string str) =>
        {
            char favouriteNumeral;
            return new Tuple<bool, string, char>(char.TryParse(str, out favouriteNumeral), "This is not a char", favouriteNumeral);
        };

        public static Func<string, Tuple<bool, string, short>> shortConverter = (string str) =>
        {
            short age;
            return new Tuple<bool, string, short>(short.TryParse(str, out age), "This is not a number", age);
        };

        public static Func<string, Tuple<bool, string, decimal>> decimalConverter = (string str) =>
        {
            decimal income;
            return new Tuple<bool, string, decimal>(decimal.TryParse(str, out income), "This is not a number", income);
        };

        public static Func<DateTime, Tuple<bool, string>> dateValidator = (DateTime dateOfBirth) =>
        {
            if (typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (dateOfBirth.CompareTo(DateTime.Now) >= 0 || dateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
                {
                    return new Tuple<bool, string>(false, "Your input date must be less then todays date and greater than 1 Jan of 1950");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (dateOfBirth.CompareTo(DateTime.Now) >= 0 || dateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
                {
                    return new Tuple<bool, string>(false, "Your input date must be less then todays date and greater than 1 Jan of 1950");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static Func<string, Tuple<bool, string>> stringValidator = (string str) =>
        {
            if (typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(str) || str.Length < 2 || str.Length > 60)
                {
                    return new Tuple<bool, string>(false, "Your input length must be more than 2 and less than 60 and not all the spaces");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(str) || str.Length < 2 || str.Length > 60)
                {
                    return new Tuple<bool, string>(false, "Your input length must be more than 2 and less than 60 and not all the spaces");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static Func<char, Tuple<bool, string>> charValidator = (char favouriteNumeral) =>
        {
            if (typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (favouriteNumeral > '9' || favouriteNumeral < '0')
                {
                    return new Tuple<bool, string>(false, "Your input numeral must be between 0 and 9");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (favouriteNumeral > '5' || favouriteNumeral < '0')
                {
                    return new Tuple<bool, string>(false, "Your input numeral must be between 0 and 5");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static Func<short, Tuple<bool, string>> shortValidator = (short age) =>
        {
            if (typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (age > 100 || age < 0)
                {
                    return new Tuple<bool, string>(false, "Your input age must be positive and less than 100");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (age > 20 || age < 0)
                {
                    return new Tuple<bool, string>(false, "Your input age must be positive and less than 20");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static Func<decimal, Tuple<bool, string>> decimalValidator = (decimal income) =>
        {
            if (typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (income > 2000000 || income < 350)
                {
                    return new Tuple<bool, string>(false, "Your input income must be more than 350 and less than 2000000");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (income > 500 || income < 0)
                {
                    return new Tuple<bool, string>(false, "Your input income must be positive and less than 500");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };
    }
}
