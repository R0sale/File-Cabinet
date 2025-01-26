using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class ImportCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("import", StringComparison.Ordinal))
            {
                this.Import(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Import(string parameters)
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
                        if (Program.fileCabinetService == null)
                        {
                            throw new ArgumentException("Service is null");
                        }

                        Program.fileCabinetService.Restore(snapshot);
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

                        if (Program.fileCabinetService is null)
                        {
                            throw new ArgumentException("The service is null");
                        }

                        Program.fileCabinetService.Restore(snapshot);
                    }

                    Console.WriteLine($"Import {args[1]}");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
