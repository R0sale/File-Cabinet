using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CustomAgeValidator : IRecordValidator
    {
        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.Age > 20 || rec.Age < 0)
            {
                throw new ArgumentException("Exception because of the incorrect age format");
            }
        }
    }
}
