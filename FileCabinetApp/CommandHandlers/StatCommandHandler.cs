using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
    /// </summary>
    /// <param name="service"></param>
    public class StatCommandHandler(IFileCabinetService service)
        : ServiceCommandHandlerBase(service)
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
            if (this.service != null)
            {
                Console.WriteLine($"{this.service.GetStat()} record(s). {this.service.GetDeletedRecords()}");
            }
        }
    }
}
