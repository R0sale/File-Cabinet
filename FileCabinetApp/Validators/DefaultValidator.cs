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
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Method for validating parameters of the record.
        /// </summary>
        /// <param name="rec">The record.</param>
        /// <exception cref="ArgumentException">Exception for all the rules.</exception>
        public void ValidateParameters(ParameterObject rec)
        {
            if (rec == null)
            {
                throw new ArgumentException("The record is null");
            }

            new FirstNameValidator(2, 60).ValidateParameters(rec);
            new LastNameValidator(2, 60).ValidateParameters(rec);
            new DateOfBirthValidator(new DateTime(01 / 01 / 1950), DateTime.Now).ValidateParameters(rec);
            new AgeValidator(0, 100).ValidateParameters(rec);
            new FavouriteNumeralValidator('0', '9').ValidateParameters(rec);
            new IncomeValidator(350, 2000000).ValidateParameters(rec);
        }
    }
}
