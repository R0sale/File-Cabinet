using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    internal static class Extensions
    {
        private static ValidationCriteria criteria;

        static Extensions()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\kvuso\\.vscode\\newtry\\new-try\\FileCabinetApp\\validation-rules.json", optional: false, reloadOnChange: true)
                .Build();

            if (Program.typeOfTheRules.Equals("default", StringComparison.OrdinalIgnoreCase))
            {
                criteria = configuration.GetSection("default").Get<ValidationCriteria>();
            }
            else
            {
                criteria = configuration.GetSection("custom").Get<ValidationCriteria>();
            }
        }

        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            builder.ValidateFirstName(criteria.FirstName.MinLength, criteria.FirstName.MaxLength);
            builder.ValidateLastName(criteria.LastName.MinLength, criteria.LastName.MaxLength);
            builder.ValidateDateOfBirth(criteria.DateOfBirth.From, criteria.DateOfBirth.To);
            return builder.Create();
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            builder.ValidateFirstName(criteria.FirstName.MinLength, criteria.FirstName.MaxLength);
            builder.ValidateLastName(criteria.LastName.MinLength, criteria.LastName.MaxLength);
            builder.ValidateDateOfBirth(criteria.DateOfBirth.From, criteria.DateOfBirth.To);
            return builder.Create();
        }
    }
}
