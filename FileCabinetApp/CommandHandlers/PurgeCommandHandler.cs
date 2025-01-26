using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler(IFileCabinetService service)
        : ServiceCommandHandlerBase(service)
    {
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("purge", StringComparison.Ordinal))
            {
                this.Purge(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Purge(string parameters)
        {
            try
            {
                if (!(this.service is FileCabinetFilesystemService))
                {
                    throw new ArgumentException("The service is not file system type");
                }

                this.service.PurgeRecords();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
