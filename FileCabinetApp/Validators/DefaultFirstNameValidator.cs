using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class DefaultFirstNameValidator : IRecordValidator
    {
        public void ValidateParameters(ParameterObject rec)
        {
            if (string.IsNullOrWhiteSpace(rec.FirstName) || rec.FirstName.Length < 2 || rec.FirstName.Length > 60)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }
        }
    }
}
