using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string? firstName, string? lastName, DateTime dateOfBirth, short age, char favouriteNumeral, decimal income)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
                {
                    throw new ArgumentException("Exception because of the incorrect first name format");
                }

                if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
                {
                    throw new ArgumentException("Exception because of the incorrect last name format");
                }

                if (dateOfBirth.CompareTo(DateTime.Now) >= 0 || dateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
                {
                    throw new ArgumentException("Exception because of the incorrect date of birth format");
                }

                if (favouriteNumeral > '9' || favouriteNumeral < '0')
                {
                    throw new ArgumentException("Exception because of the incorrect favourite numeral format");
                }

                if (age > 100 || age < 0)
                {
                    throw new ArgumentException("Exception because of the incorrect age format");
                }

                if (income > 2000000 || income < 350)
                {
                    throw new ArgumentException("Exception because of the incorrect income format");
                }

                var record = new FileCabinetRecord
                {
                    Id = this.list.Count + 1,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Age = age,
                    FavouriteNumeral = favouriteNumeral,
                    Income = income,
                };

                this.list.Add(record);
                Console.WriteLine($"Record #{this.GetStat()} is created.");

                return record.Id;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public FileCabinetRecord[] GetRecords()
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            foreach (var record in this.list)
            {
                result.Add(record);
            }

            return result.ToArray();
        }

        public int GetStat() => this.list.Count;

        public void EditRecord(int id, string? firstName, string? lastName, DateTime dateOfBirth, short age, char favouriteNumeral, decimal income)
        {
            try
            {
                if (id > this.list.Count || id < 0)
                {
                    throw new ArgumentException("There is no such a record");
                }

                var record = new FileCabinetRecord
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Age = age,
                    FavouriteNumeral = favouriteNumeral,
                    Income = income,
                };

                this.list[id - 1] = record;
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#{id} record not found");
            }
        }
    }
}
