using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

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

        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("help", StringComparison.Ordinal))
            {
                this.PrintHelp(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
