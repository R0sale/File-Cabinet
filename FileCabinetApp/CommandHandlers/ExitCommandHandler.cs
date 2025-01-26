using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> exit;

        public ExitCommandHandler(Action<bool> exitingApp)
        {
            this.exit = exitingApp;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("exit", StringComparison.Ordinal))
            {
                this.Exit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            this.exit(false);
        }
    }
}
