using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private DateTime from;

        private DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.DateOfBirth.CompareTo(this.to) >= 0 || rec.DateOfBirth.CompareTo(this.from) <= 0)
            {
                throw new ArgumentException("Exception because of the incorrect date of birth format");
            }
        }
    }
}
