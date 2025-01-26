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

            this.ValidateFirstName(rec.FirstName);
            this.ValidateLastName(rec.LastName);
            this.ValidateDateOfBirth(rec.DateOfBirth);
            this.ValidateFavouriteNumeral(rec.FavouriteNumeral);
            this.ValidateAge(rec.Age);
            this.ValidateIncome(rec.Income);
        }

        private void ValidateFirstName(string firstname)
        {
            if (string.IsNullOrWhiteSpace(firstname) || firstname.Length < 2 || firstname.Length > 60)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }
        }

        private void ValidateLastName(string lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname) || lastname.Length < 2 || lastname.Length > 60)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }
        }

        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth.CompareTo(DateTime.Now) >= 0 || dateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
            {
                throw new ArgumentException("Exception because of the incorrect date of birth format");
            }
        }

        private void ValidateFavouriteNumeral(char favouriteNumeral)
        {
            if (favouriteNumeral > '9' || favouriteNumeral < '0')
            {
                throw new ArgumentException("Exception because of the incorrect favourite numeral format");
            }
        }

        private void ValidateIncome(decimal income)
        {
            if (income > 2000000 || income < 350)
            {
                throw new ArgumentException("Exception because of the incorrect income format");
            }
        }

        private void ValidateAge(short age)
        {
            if (age > 100 || age < 0)
            {
                throw new ArgumentException("Exception because of the incorrect age format");
            }
        }
    }
}
