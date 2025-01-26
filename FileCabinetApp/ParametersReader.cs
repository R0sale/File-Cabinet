using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class ParametersReader
    {
        public static readonly Func<string, Tuple<bool, string, string>> StringConverter = (string str) =>
        {
            return new Tuple<bool, string, string>(!string.IsNullOrEmpty(str), "String is null or empty", str);
        };

        public static readonly Func<string, Tuple<bool, string, DateTime>> DateConverter = (string str) =>
        {
            DateTime dateOfBirth;
            return new Tuple<bool, string, DateTime>(DateTime.TryParseExact(str, "MM/dd/yyyy", null, DateTimeStyles.None, out dateOfBirth), "String is null or empty", dateOfBirth);
        };

        public static readonly Func<string, Tuple<bool, string, char>> CharConverter = (string str) =>
        {
            char favouriteNumeral;
            return new Tuple<bool, string, char>(char.TryParse(str, out favouriteNumeral), "This is not a char", favouriteNumeral);
        };

        public static readonly Func<string, Tuple<bool, string, short>> ShortConverter = (string str) =>
        {
            short age;
            return new Tuple<bool, string, short>(short.TryParse(str, out age), "This is not a number", age);
        };

        public static readonly Func<string, Tuple<bool, string, decimal>> DecimalConverter = (string str) =>
        {
            decimal income;
            return new Tuple<bool, string, decimal>(decimal.TryParse(str, out income), "This is not a number", income);
        };

        public static readonly Func<DateTime, Tuple<bool, string>> DateValidator = (DateTime dateOfBirth) =>
        {
            if (Program.typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (dateOfBirth.CompareTo(DateTime.Now) >= 0 || dateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
                {
                    return new Tuple<bool, string>(false, "Your input date must be less then todays date and greater than 1 Jan of 1950");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (dateOfBirth.CompareTo(DateTime.Now) >= 0 || dateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
                {
                    return new Tuple<bool, string>(false, "Your input date must be less then todays date and greater than 1 Jan of 1950");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static readonly Func<string, Tuple<bool, string>> StringValidator = (string str) =>
        {
            if (Program.typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(str) || str.Length < 2 || str.Length > 60)
                {
                    return new Tuple<bool, string>(false, "Your input length must be more than 2 and less than 60 and not all the spaces");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(str) || str.Length < 2 || str.Length > 60)
                {
                    return new Tuple<bool, string>(false, "Your input length must be more than 2 and less than 60 and not all the spaces");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static readonly Func<char, Tuple<bool, string>> CharValidator = (char favouriteNumeral) =>
        {
            if (Program.typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (favouriteNumeral > '9' || favouriteNumeral < '0')
                {
                    return new Tuple<bool, string>(false, "Your input numeral must be between 0 and 9");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (favouriteNumeral > '5' || favouriteNumeral < '0')
                {
                    return new Tuple<bool, string>(false, "Your input numeral must be between 0 and 5");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static readonly Func<short, Tuple<bool, string>> ShortValidator = (short age) =>
        {
            if (Program.typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (age > 100 || age < 0)
                {
                    return new Tuple<bool, string>(false, "Your input age must be positive and less than 100");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (age > 20 || age < 0)
                {
                    return new Tuple<bool, string>(false, "Your input age must be positive and less than 20");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static readonly Func<decimal, Tuple<bool, string>> DecimalValidator = (decimal income) =>
        {
            if (Program.typeOfTheRules.Equals("default", StringComparison.Ordinal))
            {
                if (income > 2000000 || income < 350)
                {
                    return new Tuple<bool, string>(false, "Your input income must be more than 350 and less than 2000000");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
            else
            {
                if (income > 500 || income < 0)
                {
                    return new Tuple<bool, string>(false, "Your input income must be positive and less than 500");
                }
                else
                {
                    return new Tuple<bool, string>(true, "Everything is fine");
                }
            }
        };

        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                if (input == null)
                {
                    throw new ArgumentException("The input is null");
                }

                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
