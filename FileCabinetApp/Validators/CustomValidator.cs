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

            new CustomFirstNameValidator().ValidateParameters(rec);
            new CustomLastNameValidator().ValidateParameters(rec);
            new CustomDateOfBirthValidator().ValidateParameters(rec);
            new CustomFavouriteNumeralValidator().ValidateParameters(rec);
            new CustomAgeValidator().ValidateParameters(rec);
            new CustomIncomeValidator().ValidateParameters(rec);
        }
    }
}
