using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CreateCommandHandler(IFileCabinetService service)
        : ServiceCommandHandlerBase(service)
    {

        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("create", StringComparison.Ordinal))
            {
                this.Create(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Create(string parameters)
        {
            int counter = 0;
            do
            {
                Console.Write("First name: ");
                string? firstname = ParametersReader.ReadInput(ParametersReader.StringConverter, ParametersReader.StringValidator);

                Console.Write("last name: ");
                string? lastname = ParametersReader.ReadInput(ParametersReader.StringConverter, ParametersReader.StringValidator);

                Console.Write("Date of birth: ");
                DateTime dateOfBirth = ParametersReader.ReadInput(ParametersReader.DateConverter, ParametersReader.DateValidator);

                Console.Write("Age: ");
                short age = ParametersReader.ReadInput(ParametersReader.ShortConverter, ParametersReader.ShortValidator);

                Console.Write("Favourite numeral: ");
                char favouriteNumeral = ParametersReader.ReadInput(ParametersReader.CharConverter, ParametersReader.CharValidator);

                Console.Write("Income: ");
                decimal income = ParametersReader.ReadInput(ParametersReader.DecimalConverter, ParametersReader.DecimalValidator);

                ParameterObject obj = new ParameterObject()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    DateOfBirth = dateOfBirth,
                    Age = age,
                    FavouriteNumeral = favouriteNumeral,
                    Income = income,
                };

                if (this.service != null)
                {
                    counter = this.service.CreateRecord(obj);
                }
            }
            while (counter < 1);
        }
    }
}
