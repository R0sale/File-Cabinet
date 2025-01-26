using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("remove", StringComparison.Ordinal))
            {
                this.Remove(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Remove(string parameters)
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

                if (Program.fileCabinetService is null)
                {
                    throw new ArgumentException("The service is null");
                }

                Program.fileCabinetService.RemoveRecords(id);
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
    }
}
