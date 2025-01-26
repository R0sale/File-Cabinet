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
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Method for validating parameters.
        /// </summary>
        /// <param name="rec">The record.</param>
        /// <exception cref="ArgumentException">Exception for the custim rules.</exception>
        public void ValidateParameters(ParameterObject rec)
        {
            if (rec == null)
            {
                throw new ArgumentException("The record is null");
            }

            new FirstNameValidator(2, 60).ValidateParameters(rec);
            new LastNameValidator(2, 60).ValidateParameters(rec);
            new DateOfBirthValidator(new DateTime(01 / 01 / 1950), DateTime.Now).ValidateParameters(rec);
            new AgeValidator(0, 20).ValidateParameters(rec);
            new FavouriteNumeralValidator('0', '5').ValidateParameters(rec);
            new IncomeValidator(0, 500).ValidateParameters(rec);
        }
    }
}
