using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        private ValidationCriteria criteria;

        public ValidatorBuilder()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\kvuso\\.vscode\\newtry\\new-try\\FileCabinetApp\\validation-rules.json", optional: false, reloadOnChange: true)
                .Build();

            if (Program.typeOfTheRules.Equals("default", StringComparison.OrdinalIgnoreCase))
            {
                this.criteria = configuration.GetSection("default").Get<ValidationCriteria>();
            }
            else
            {
                this.criteria = configuration.GetSection("custom").Get<ValidationCriteria>();
            }
        }

        public void ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
        }

        public void ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new LastNameValidator(minLength, maxLength));
        }

        public void ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
        }

        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
