using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class EditCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("The request is null");
            }

            if (request.Command.Equals("edit", StringComparison.Ordinal))
            {
                this.Edit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Edit(string parameters)
        {
            int counter = 0;
            do
            {
                int numberOfTheRecord;

                if (string.IsNullOrEmpty(parameters))
                {
                    Console.WriteLine("Write the parameters in appropriate way");
                    return;
                }

                if (!int.TryParse(parameters, out numberOfTheRecord))
                {
                    Console.WriteLine("Write the parameters in appropriate way");
                    return;
                }

                if (Program.fileCabinetService != null)
                {
                    if (numberOfTheRecord <= Program.fileCabinetService.GetStat())
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

                        counter = Program.fileCabinetService.EditRecord(numberOfTheRecord, obj);
                    }
                    else
                    {
                        Console.WriteLine($"#{numberOfTheRecord} record is not found.");
                        break;
                    }
                }
            }
            while (counter < 1);
        }
    }
}
