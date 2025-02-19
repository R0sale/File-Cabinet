using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FirstNameValidator : IRecordValidator
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public FirstNameValidator(int minLength, int maxLength)
        {
            this.MinLength = minLength;
            this.MaxLength = maxLength;
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (string.IsNullOrWhiteSpace(rec.FirstName) || rec.FirstName.Length < this.MinLength || rec.FirstName.Length > this.MaxLength)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }
        }
    }
}
