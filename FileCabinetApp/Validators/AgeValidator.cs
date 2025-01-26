using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class AgeValidator : IRecordValidator
    {
        private short minAge;

        private short maxAge;

        public AgeValidator(short minAge, short maxAge)
        {
            this.minAge = minAge;
            this.maxAge = maxAge;
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.Age > this.maxAge || rec.Age < this.minAge)
            {
                throw new ArgumentException("Exception because of the incorrect age format");
            }
        }
    }
}
