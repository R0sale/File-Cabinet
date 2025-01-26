using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new LastNameValidator(minLength, maxLength));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }

        public IRecordValidator CreateDefault()
        {
            return new ValidatorBuilder().ValidateFirstName(2, 60).ValidateLastName(2, 60).ValidateDateOfBirth(new DateTime(01 / 01 / 1950), DateTime.Now).Create();
        }

        public IRecordValidator CreateCustom()
        {
            return new ValidatorBuilder().ValidateFirstName(2, 60).ValidateLastName(2, 60).ValidateDateOfBirth(new DateTime(01 / 01 / 1950), DateTime.Now).Create();
        }
    }
}
