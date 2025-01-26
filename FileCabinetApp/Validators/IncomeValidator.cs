using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class IncomeValidator : IRecordValidator
    {
        private decimal minIncome;

        private decimal maxIncome;

        public IncomeValidator(decimal minIncome, decimal maxIncome) 
        {
            this.minIncome = minIncome;
            this.maxIncome = maxIncome;
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.Income > this.maxIncome || rec.Income < this.minIncome)
            {
                throw new ArgumentException("Exception because of the incorrect income format");
            }
        }
    }
}
