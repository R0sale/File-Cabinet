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

            new DefaultFirstNameValidator().ValidateParameters(rec);
            new DefaultLastNameValidator().ValidateParameters(rec);
            new DefaultDateOfBirthValidator().ValidateParameters(rec);
            new DefaultFavouriteNumeralValidator().ValidateParameters(rec);
            new DefaultAgeValidator().ValidateParameters(rec);
            new DefaultIncomeValidator().ValidateParameters(rec);
        }
    }
}
