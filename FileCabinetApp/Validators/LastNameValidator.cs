using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class LastNameValidator : IRecordValidator
    {
        private int minLength;

        private int maxLength;

        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (string.IsNullOrWhiteSpace(rec.LastName) || rec.LastName.Length < this.minLength || rec.LastName.Length > this.maxLength)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }
        }
    }
}
