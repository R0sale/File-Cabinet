using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CustomIncomeValidator : IRecordValidator
    {
        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.Income > 500 || rec.Income < 0)
            {
                throw new ArgumentException("Exception because of the incorrect income format");
            }
        }
    }
}
