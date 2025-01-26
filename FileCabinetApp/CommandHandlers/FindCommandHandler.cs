using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("find", StringComparison.Ordinal))
            {
                this.Find(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Find(string arguments)
        {
            try
            {
                string[] args = arguments.Split(' ');
                if (args.Length != 2)
                {
                    throw new ArgumentException("Please write 2 arguments");
                }

                if (Program.fileCabinetService == null)
                {
                    throw new ArgumentException("The service is null");
                }

                if (args[0].Equals("firstname", StringComparison.OrdinalIgnoreCase))
                {
                    IReadOnlyCollection<FileCabinetRecord> records = Program.fileCabinetService.FindByFirstName(args[1]);
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
                    IReadOnlyCollection<FileCabinetRecord> records = Program.fileCabinetService.FindByLastName(args[1]);
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
                        IReadOnlyCollection<FileCabinetRecord> records = Program.fileCabinetService.FindByDateOfBirth(dateOfBirth);

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
