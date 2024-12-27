using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Kirill Vusov";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "shows the number of records", "The 'stat' command shows the number of records" },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record" },
            new string[] { "list", "shows all the records", "The 'list' command shows all the records" },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            string? firstname;

            do
            {
                Console.Write("First name: ");
                firstname = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(firstname) || firstname.Length < 2 || firstname.Length > 60);

            string? lastname;

            do
            {
                Console.Write("last name: ");
                lastname = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(lastname) || lastname.Length < 2 || lastname.Length > 60);

            DateTime dateOfBirth;

            do
            {
                Console.Write("Date of birth: ");
                DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dateOfBirth);
            }
            while (dateOfBirth.CompareTo(DateTime.Now) >= 0 || dateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0);

            short age;

            do
            {
                Console.Write("Age: ");
                short.TryParse(Console.ReadLine(), null, out age);
            }
            while (age > 100 || age < 0);

            char favouriteNumeral;
            do
            {
                Console.Write("Favourite numeral: ");
                _ = char.TryParse(Console.ReadLine(), out favouriteNumeral);
            }
            while (favouriteNumeral > '9' || favouriteNumeral < '0');

            decimal income;

            do
            {
                Console.Write("Income: ");
                _ = decimal.TryParse(Console.ReadLine(), out income);
            }
            while (income > 2000000 || income < 350);

            fileCabinetService.CreateRecord(firstname, lastname, dateOfBirth, age, favouriteNumeral, income);
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] records = fileCabinetService.GetRecords();

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", DateTimeFormatInfo.InvariantInfo)}, {record.Age}, {record.FavouriteNumeral}, {record.Income}");
            }
        }
    }
}