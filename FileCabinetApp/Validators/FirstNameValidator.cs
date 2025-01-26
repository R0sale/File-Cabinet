using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FirstNameValidator : IRecordValidator
    {
        private int minLength;

        private int maxLength;

        public FirstNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength; 
            this.maxLength = maxLength; 
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (string.IsNullOrWhiteSpace(rec.FirstName) || rec.FirstName.Length < this.minLength || rec.FirstName.Length > this.maxLength)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }
        }
    }
}
