using System.Diagnostics.Metrics;
using System.Globalization;

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
        private static FileCabinetService? fileCabinetService;

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
        };

        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="args">Just arguments.</param>
        public static void Main(string[] args)
        {
            string typeOfTheRules = "default";

            if (args != null)
            {
                if (args.Length > 0)
                {
                    if (args[0].StartsWith("--validation-rules=", StringComparison.OrdinalIgnoreCase))
                    {
                        if (args[0].Split('=')[1].Equals("custom", StringComparison.OrdinalIgnoreCase))
                        {
                            fileCabinetService = new FileCabinetService(new CustomValidator());
                            typeOfTheRules = "custom";
                        }
                    }
                    else if (args[0].Equals("-v", StringComparison.OrdinalIgnoreCase))
                    {
                        if (args[1].Equals("custom", StringComparison.OrdinalIgnoreCase))
                        {
                            fileCabinetService = new FileCabinetService(new CustomValidator());
                            typeOfTheRules = "custom";
                        }
                    }
                }
            }

            fileCabinetService = new FileCabinetService(new DefaultValidator());

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
                var recordsCount = Program.fileCabinetService.GetStat();
                Console.WriteLine($"{recordsCount} record(s).");
            }
        }

        private static void Create(string parameters)
        {
            int counter = 0;
            do
            {
                Console.Write("First name: ");
                string? firstname = Console.ReadLine();

                Console.Write("last name: ");
                string? lastname = Console.ReadLine();

                DateTime dateOfBirth;
                Console.Write("Date of birth: ");
                DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dateOfBirth);

                short age;
                Console.Write("Age: ");
                short.TryParse(Console.ReadLine(), null, out age);

                char favouriteNumeral;
                Console.Write("Favourite numeral: ");
                _ = char.TryParse(Console.ReadLine(), out favouriteNumeral);

                decimal income;
                Console.Write("Income: ");
                _ = decimal.TryParse(Console.ReadLine(), out income);

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
                        string? firstname = Console.ReadLine();

                        Console.Write("Last name: ");
                        string? lastname = Console.ReadLine();

                        DateTime dateOfBirth;
                        Console.Write("Date of birth: ");
                        DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dateOfBirth);

                        char favouriteNumeral;
                        Console.Write("Favourite numeral: ");
                        _ = char.TryParse(Console.ReadLine(), out favouriteNumeral);

                        short age;
                        Console.Write("Age: ");
                        short.TryParse(Console.ReadLine(), null, out age);

                        decimal income;
                        Console.Write("Income: ");
                        _ = decimal.TryParse(Console.ReadLine(), out income);

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
    }
}
