using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class LastNameValidator : IRecordValidator
    {
        public int MinLength {  get; set; }

        public int MaxLength { get; set; }

        public LastNameValidator(int minLength, int maxLength)
        {
            this.MinLength = minLength;
            this.MaxLength = maxLength;
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (string.IsNullOrWhiteSpace(rec.LastName) || rec.LastName.Length < this.MinLength || rec.LastName.Length > this.MaxLength)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }
        }
    }
}
