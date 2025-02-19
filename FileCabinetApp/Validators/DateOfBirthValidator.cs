using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class DateOfBirthValidator : IRecordValidator
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.From = from;
            this.To = to;
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.DateOfBirth.CompareTo(this.To) >= 0 || rec.DateOfBirth.CompareTo(this.From) <= 0)
            {
                throw new ArgumentException("Exception because of the incorrect date of birth format");
            }
        }
    }
}
