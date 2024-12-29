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

            if (string.IsNullOrWhiteSpace(rec.FirstName) || rec.FirstName.Length < 2 || rec.FirstName.Length > 60)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }

            if (string.IsNullOrWhiteSpace(rec.LastName) || rec.LastName.Length < 2 || rec.LastName.Length > 60)
            {
                throw new ArgumentException("Exception because of the incorrect last name format");
            }

            if (rec.DateOfBirth.CompareTo(DateTime.Now) >= 0 || rec.DateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
            {
                throw new ArgumentException("Exception because of the incorrect date of birth format");
            }

            if (rec.FavouriteNumeral > '9' || rec.FavouriteNumeral < '0')
            {
                throw new ArgumentException("Exception because of the incorrect favourite numeral format");
            }

            if (rec.Age > 100 || rec.Age < 0)
            {
                throw new ArgumentException("Exception because of the incorrect age format");
            }

            if (rec.Income > 2000000 || rec.Income < 350)
            {
                throw new ArgumentException("Exception because of the incorrect income format");
            }
        }
    }
}
