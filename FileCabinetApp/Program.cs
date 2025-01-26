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
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
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
            var commandHandler = new CommandHandler();
            return commandHandler;
        }
    }
}
