using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("stat", StringComparison.Ordinal))
            {
                this.Stat(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Stat(string parameters)
        {
            if (Program.fileCabinetService != null)
            {
                Console.WriteLine($"{Program.fileCabinetService.GetStat()} record(s). {Program.fileCabinetService.GetDeletedRecords()}");
            }
        }
    }
}
