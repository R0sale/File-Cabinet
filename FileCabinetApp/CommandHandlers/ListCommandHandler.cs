using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        //public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer)
        //    : base(service)
        //{
        //    this.printer = printer;
        //}

        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("list", StringComparison.Ordinal))
            {
                IEnumerable<FileCabinetRecord> records = this.List(request.Parameters);
                this.printer(records);
            }
            else
            {
                base.Handle(request);
            }
        }

        private IReadOnlyCollection<FileCabinetRecord> List(string parameters)
        {
            if (this.service != null)
            {
                IReadOnlyCollection<FileCabinetRecord> records = this.service.GetRecords();
                return records;
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }
    }
}
