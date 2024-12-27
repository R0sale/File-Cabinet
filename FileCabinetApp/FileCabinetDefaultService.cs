using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for default validation.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Overriding the abstract method from parent class.
        /// </summary>
        /// <param name="rec">Record.</param>
        /// <exception cref="ArgumentException">Exception for validation.</exception>
        protected override void ValidateParameters(ParameterObject rec)
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
