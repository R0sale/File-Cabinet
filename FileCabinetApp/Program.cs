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
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
#pragma warning disable CA1859
        private static IFileCabinetService? fileCabinetService;
#pragma warning restore CA1859
        private static string typeOfTheRules = "default";

        private static Func<string, Tuple<bool, string, string>> stringConverter = (string str) =>
        {
            return new Tuple<bool, string, string>(!string.IsNullOrEmpty(str), "String is null or empty", str);
        };

        private static Func<string, Tuple<bool, string, DateTime>> dateConverter = (string str) =>
        {
            DateTime dateOfBirth;
            return new Tuple<bool, string, DateTime>(DateTime.TryParseExact(str, "MM/dd/yyyy", null, DateTimeStyles.None, out dateOfBirth), "String is null or empty", dateOfBirth);
        };

        private static Func<string, Tuple<bool, string, char>> charConverter = (string str) =>
        {
            char favouriteNumeral;
            return new Tuple<bool, string, char>(char.TryParse(str, out favouriteNumeral), "This is not a char", favouriteNumeral);
        };

        private static Func<string, Tuple<bool, string, short>> shortConverter = (string str) =>
        {
            short age;
            return new Tuple<bool, string, short>(short.TryParse(str, out age), "This is not a number", age);
        };

        private static Func<string, Tuple<bool, string, decimal>> decimalConverter = (string str) =>
        {
            decimal income;
            return new Tuple<bool, string, decimal>(decimal.TryParse(str, out income), "This is not a number", income);
        };

        private static Func<DateTime, Tuple<bool, string>> dateValidator = (DateTime dateOfBirth) =>
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

        private static Func<string, Tuple<bool, string>> stringValidator = (string str) =>
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

        private static Func<char, Tuple<bool, string>> charValidator = (char favouriteNumeral) =>
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

        private static Func<short, Tuple<bool, string>> shortValidator = (short age) =>
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

        private static Func<decimal, Tuple<bool, string>> decimalValidator = (decimal income) =>
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

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "shows the number of records", "The 'stat' command shows the number of records" },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record" },
            new string[] { "list", "shows all the records", "The 'list' command shows all the records" },
            new string[] { "edit", "edits the specified record", "The 'edit' command edits the specified record" },
            new string[] { "find", "finds the record by specified parameters : firstname or lastname or dateofbirth", "The 'find' command finds the record by specified parameters : firstname or lastname or dateofbirth" },
            new string[] { "export csv/xml", "exports the data of the service into the csv/xml file", "The 'export csv/xml' command exports the data of the service into the csv/xml file" },
            new string[] { "import csv/xml", "imports the data of the csv/xml file into file depot", "The 'import csv/xml' command imports the data of the csv file into file depot" },
            new string[] { "remove", "removes the records", "The 'remove' command removes the records" },
            new string[] { "purge", "removes all the records that are 'removed' from the filing system", "The 'purge' command removes all the records that are 'removed' from the filing system" },
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

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
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

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            if (fileCabinetService != null)
            {
                Console.WriteLine($"{fileCabinetService.GetStat()} record(s). {fileCabinetService.GetDeletedRecords()}");
            }
        }

        private static void Create(string parameters)
        {
            int counter = 0;
            do
            {
                Console.Write("First name: ");
                string? firstname = ReadInput(stringConverter, stringValidator);

                Console.Write("last name: ");
                string? lastname = ReadInput(stringConverter, stringValidator);

                Console.Write("Date of birth: ");
                DateTime dateOfBirth = ReadInput(dateConverter, dateValidator);

                Console.Write("Age: ");
                short age = ReadInput(shortConverter, shortValidator);

                Console.Write("Favourite numeral: ");
                char favouriteNumeral = ReadInput(charConverter, charValidator);

                Console.Write("Income: ");
                decimal income = ReadInput(decimalConverter, decimalValidator);

                ParameterObject obj = new ParameterObject()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    DateOfBirth = dateOfBirth,
                    Age = age,
                    FavouriteNumeral = favouriteNumeral,
                    Income = income,
                };

                if (fileCabinetService != null)
                {
                    counter = fileCabinetService.CreateRecord(obj);
                }
            }
            while (counter < 1);
        }

        private static void List(string parameters)
        {
            if (fileCabinetService != null)
            {
                IReadOnlyCollection<FileCabinetRecord> records = fileCabinetService.GetRecords();
                foreach (var record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", DateTimeFormatInfo.InvariantInfo)}, {record.Age}, {record.FavouriteNumeral}, {record.Income}");
                }
            }
        }

        private static void Edit(string parameters)
        {
            int counter = 0;
            do
            {
                int numberOfTheRecord;

                if (string.IsNullOrEmpty(parameters))
                {
                    Console.WriteLine("Write the parameters in appropriate way");
                    return;
                }

                if (!int.TryParse(parameters, out numberOfTheRecord))
                {
                    Console.WriteLine("Write the parameters in appropriate way");
                    return;
                }

                if (fileCabinetService != null)
                {
                    if (numberOfTheRecord <= fileCabinetService.GetStat())
                    {
                        Console.Write("First name: ");
                        string? firstname = ReadInput(stringConverter, stringValidator);

                        Console.Write("last name: ");
                        string? lastname = ReadInput(stringConverter, stringValidator);

                        Console.Write("Date of birth: ");
                        DateTime dateOfBirth = ReadInput(dateConverter, dateValidator);

                        Console.Write("Age: ");
                        short age = ReadInput(shortConverter, shortValidator);

                        Console.Write("Favourite numeral: ");
                        char favouriteNumeral = ReadInput(charConverter, charValidator);

                        Console.Write("Income: ");
                        decimal income = ReadInput(decimalConverter, decimalValidator);

                        ParameterObject obj = new ParameterObject()
                        {
                            FirstName = firstname,
                            LastName = lastname,
                            DateOfBirth = dateOfBirth,
                            Age = age,
                            FavouriteNumeral = favouriteNumeral,
                            Income = income,
                        };

                        counter = fileCabinetService.EditRecord(numberOfTheRecord, obj);
                    }
                    else
                    {
                        Console.WriteLine($"#{numberOfTheRecord} record is not found.");
                        break;
                    }
                }
            }
            while (counter < 1);
        }

        private static void Find(string arguments)
        {
            try
            {
                string[] args = arguments.Split(' ');
                if (args.Length != 2)
                {
                    throw new ArgumentException("Please write 2 arguments");
                }

                if (fileCabinetService == null)
                {
                    throw new ArgumentException("The service is null");
                }

                if (args[0].Equals("firstname", StringComparison.OrdinalIgnoreCase))
                {
                    IReadOnlyCollection<FileCabinetRecord> records = fileCabinetService.FindByFirstName(args[1]);
                    if (records != null)
                    {
                        foreach (FileCabinetRecord record in records)
                        {
                            Console.WriteLine($"#{record.Id} {record.FirstName} {record.LastName} {record.DateOfBirth.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)}");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("There is no such firstname in the list");
                    }
                }
                else if (args[0].Equals("lastname", StringComparison.OrdinalIgnoreCase))
                {
                    IReadOnlyCollection<FileCabinetRecord> records = fileCabinetService.FindByLastName(args[1]);
                    if (records != null)
                    {
                        foreach (FileCabinetRecord record in records)
                        {
                            Console.WriteLine($"#{record.Id} {record.FirstName} {record.LastName} {record.DateOfBirth.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)}");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("There is no such lastname in the list");
                    }
                }
                else if (args[0].Equals("dateofbirth", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime dateOfBirth;
                    if (DateTime.TryParseExact(args[1], "yyyy-MMM-dd", new CultureInfo("En-en"), DateTimeStyles.None, out dateOfBirth))
                    {
                        IReadOnlyCollection<FileCabinetRecord> records = fileCabinetService.FindByDateOfBirth(dateOfBirth);

                        if (records != null)
                        {
                            foreach (FileCabinetRecord record in records)
                            {
                                Console.WriteLine($"#{record.Id} {record.FirstName} {record.LastName} {record.DateOfBirth.ToString("yyyy-MMM-dd", new CultureInfo("En-en"))}");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("There is no such date in the list");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Please, write the date in the correct format");
                    }
                }
                else
                {
                    throw new ArgumentException("The arguments are not in appropriate way");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

        private static void Export(string parameters)
        {
            try
            {
                string[] args = parameters.Split(' ');

                if (args.Length != 2)
                {
                    throw new ArgumentException("The arguments are not correct");
                }

                string[] arguments = args[1].Split('.');

                if (arguments[1].Equals("csv", StringComparison.Ordinal))
                {
                    if (File.Exists(args[1]))
                    {
                        string? input;
                        do
                        {
                            Console.WriteLine($"File {args[1]} is already exist, do you want to rewrite it? [Y/N]");
                            input = Console.ReadLine();
                            if (!string.IsNullOrEmpty(input) && input.Equals("N", StringComparison.Ordinal))
                            {
                                return;
                            }
                        }
                        while (string.IsNullOrEmpty(input) || (!input.Equals("N", StringComparison.Ordinal) && !input.Equals("Y", StringComparison.Ordinal)));
                    }

                    using (StreamWriter writer = new StreamWriter(args[1]))
                    {
                        if (fileCabinetService is null)
                        {
                            throw new ArgumentException("the fileCabinetService is null");
                        }

                        FileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();

                        snapshot.SaveToCsv(writer);
                    }

                    Console.WriteLine($"All records are exported to file {args[1]}.");
                }
                else if (arguments[1].Equals("xml", StringComparison.Ordinal))
                {
                    if (File.Exists(args[1]))
                    {
                        string? input;
                        do
                        {
                            Console.WriteLine($"File {args[1]} is already exist, do you want to rewrite it? [Y/N]");
                            input = Console.ReadLine();
                            if (!string.IsNullOrEmpty(input) && input.Equals("N", StringComparison.Ordinal))
                            {
                                return;
                            }
                        }
                        while (string.IsNullOrEmpty(input) || (!input.Equals("N", StringComparison.Ordinal) && !input.Equals("Y", StringComparison.Ordinal)));
                    }

                    using (StreamWriter writer = new StreamWriter(args[1]))
                    {
                        if (fileCabinetService is null)
                        {
                            throw new ArgumentException("the fileCabinetService is null");
                        }

                        FileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();

                        snapshot.SaveToXml(writer);
                    }

                    Console.WriteLine($"All records are exported to file {args[1]}.");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Import(string parameters)
        {
            try
            {
                string[] args = parameters.Split(' ');

                if (args.Length != 2)
                {
                    throw new ArgumentException("The arguments are not correct");
                }

                if (args[0].Equals("csv", StringComparison.Ordinal))
                {
                    if (!args[1].Substring(args[1].Length - 3).Equals("csv", StringComparison.Ordinal))
                    {
                        throw new ArgumentException("The file format is not correct");
                    }

                    if (!File.Exists(args[1]))
                    {
                        throw new ArgumentException("The file does not exist");
                    }

                    using (StreamReader reader = new StreamReader(args[1]))
                    {
                        FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(Array.Empty<FileCabinetRecord>());
                        snapshot.LoadFromCsv(reader);
                        if (fileCabinetService == null)
                        {
                            throw new ArgumentException("Service is null");
                        }

                        fileCabinetService.Restore(snapshot);
                    }

                    Console.WriteLine($"Import {args[1]}");
                }
                else if (args[0].Equals("xml", StringComparison.Ordinal))
                {
                    if (!args[1].Substring(args[1].Length - 3).Equals("xml", StringComparison.Ordinal))
                    {
                        throw new ArgumentException("The file format is not correct");
                    }

                    if (!File.Exists(args[1]))
                    {
                        throw new ArgumentException("The file does not exists");
                    }

                    using (FileStream stream = new FileStream(args[1], FileMode.Open))
                    {
                        FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(Array.Empty<FileCabinetRecord>());

                        snapshot.LoadFromXml(stream);

                        if (fileCabinetService is null)
                        {
                            throw new ArgumentException("The service is null");
                        }

                        fileCabinetService.Restore(snapshot);
                    }

                    Console.WriteLine($"Import {args[1]}");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Remove(string parameters)
        {
            try
            {
                string[] args = parameters.Split(' ');
                int id = -1;

                if (args.Length != 1)
                {
                    throw new ArgumentException("The arguments are not correct");
                }

                id = int.Parse(args[0], new CultureInfo("En-en"));

                if (fileCabinetService is null)
                {
                    throw new ArgumentException("The service is null");
                }

                fileCabinetService.RemoveRecords(id);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"{ex.Message}. The correct format is integer.");
            }
        }

        private static void Purge(string parameters)
        {
            try
            {
                if (!(fileCabinetService is FileCabinetFilesystemService))
                {
                    throw new ArgumentException("The service is not file system type");
                }

                fileCabinetService.PurgeRecords();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
