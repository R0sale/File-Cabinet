using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CompositeValidator
    {
        private List<IRecordValidator> validators;

        protected CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = validators.ToList();
        }

        public void ValidateParameters(ParameterObject rec)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(rec);
            }
        }
    }
}
