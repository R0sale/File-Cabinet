using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected IFileCabinetService service;

        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.service = service;
        }
    }
}
