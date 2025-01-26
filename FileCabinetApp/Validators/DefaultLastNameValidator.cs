using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class DefaultLastNameValidator : IRecordValidator
    {
        public void ValidateParameters(ParameterObject rec)
        {
            if (string.IsNullOrWhiteSpace(rec.LastName) || rec.LastName.Length < 2 || rec.LastName.Length > 60)
            {
                throw new ArgumentException("Exception because of the incorrect first name format");
            }
        }
    }
}
