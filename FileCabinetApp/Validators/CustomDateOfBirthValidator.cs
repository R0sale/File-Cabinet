using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CustomDateOfBirthValidator : IRecordValidator
    {
        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.DateOfBirth.CompareTo(DateTime.Now) >= 0 || rec.DateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
            {
                throw new ArgumentException("Exception because of the incorrect date of birth format");
            }
        }
    }
}
