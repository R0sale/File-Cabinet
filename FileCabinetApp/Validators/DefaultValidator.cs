using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Stratagy class for validating data by default rules.
    /// </summary>
    public class DefaultValidator : CompositeValidator, IRecordValidator
    {
        public DefaultValidator()
            : base (new IRecordValidator[]
            {
                new FirstNameValidator(2, 60),
                new LastNameValidator(2, 60),
                new DateOfBirthValidator(new DateTime(01 / 01 / 1950), DateTime.Now),
                new AgeValidator(0, 100),
                new FavouriteNumeralValidator('0', '9'),
                new IncomeValidator(350, 2000000),
            })
        {
        }
    }
}
