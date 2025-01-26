using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class DefaultAgeValidator : IRecordValidator
    {
        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.Age > 100 || rec.Age < 0)
            {
                throw new ArgumentException("Exception because of the incorrect favourite numeral format");
            }
        }
    }
}
