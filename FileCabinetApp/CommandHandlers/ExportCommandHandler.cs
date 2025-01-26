using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler(IFileCabinetService service)
        : ServiceCommandHandlerBase(service)
    {
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("export", StringComparison.Ordinal))
            {
                this.Export(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Export(string parameters)
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
                        if (this.service is null)
                        {
                            throw new ArgumentException("the fileCabinetService is null");
                        }

                        FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();

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
                        if (this.service is null)
                        {
                            throw new ArgumentException("the fileCabinetService is null");
                        }

                        FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();

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
    }
}
