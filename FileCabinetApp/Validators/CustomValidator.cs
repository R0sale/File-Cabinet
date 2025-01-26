using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Strategy class for validating data by custom rules.
    /// </summary>
    public class CustomValidator : CompositeValidator, IRecordValidator
    {
        public CustomValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(2, 60),
                new LastNameValidator(2, 60),
                new DateOfBirthValidator(new DateTime(01 / 01 / 1950), DateTime.Now),
                new AgeValidator(0, 20),
                new FavouriteNumeralValidator('0', '5'),
                new IncomeValidator(0, 500),
            })
        {
        }
    }
}
