using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// FileCabinetService class is maintaining the whole records and set methods to use the required behaviour.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// This method is creating the record.
        /// </summary>
        /// <param name="rec">Record.</param>
        /// <returns>Identify of the record.</returns>
        public int CreateRecord(ParameterObject rec)
        {
            try
            {
                if (rec == null)
                {
                    throw new ArgumentException("The record is null");
                }

                if (string.IsNullOrWhiteSpace(rec.FirstName) || rec.FirstName.Length < 2 || rec.FirstName.Length > 60)
                {
                    throw new ArgumentException("Exception because of the incorrect first name format");
                }

                if (string.IsNullOrWhiteSpace(rec.LastName) || rec.LastName.Length < 2 || rec.LastName.Length > 60)
                {
                    throw new ArgumentException("Exception because of the incorrect last name format");
                }

                if (rec.DateOfBirth.CompareTo(DateTime.Now) >= 0 || rec.DateOfBirth.CompareTo(new DateTime(01 / 01 / 1950)) <= 0)
                {
                    throw new ArgumentException("Exception because of the incorrect date of birth format");
                }

                if (rec.FavouriteNumeral > '9' || rec.FavouriteNumeral < '0')
                {
                    throw new ArgumentException("Exception because of the incorrect favourite numeral format");
                }

                if (rec.Age > 100 || rec.Age < 0)
                {
                    throw new ArgumentException("Exception because of the incorrect age format");
                }

                if (rec.Income > 2000000 || rec.Income < 350)
                {
                    throw new ArgumentException("Exception because of the incorrect income format");
                }

                var record = new FileCabinetRecord
                {
                    Id = this.list.Count + 1,
                    FirstName = rec.FirstName,
                    LastName = rec.LastName,
                    DateOfBirth = rec.DateOfBirth,
                    Age = rec.Age,
                    FavouriteNumeral = rec.FavouriteNumeral,
                    Income = rec.Income,
                };

                this.list.Add(record);

                foreach (var firstname in this.firstNameDictionary)
                {
                    if (rec.FirstName == firstname.Key)
                    {
                        firstname.Value.Add(record);
                        Console.WriteLine($"Record #{this.GetStat()} is created.");
                        return record.Id;
                    }
                }

                foreach (var lastname in this.lastNameDictionary)
                {
                    if (rec.LastName == lastname.Key)
                    {
                        lastname.Value.Add(record);
                        Console.WriteLine($"Record #{this.GetStat()} is created.");
                        return record.Id;
                    }
                }

                foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
                {
                    if (rec.DateOfBirth == dateOfBirthFromDictionary.Key)
                    {
                        dateOfBirthFromDictionary.Value.Add(record);
                        Console.WriteLine($"Record #{this.GetStat()} is created.");
                        return record.Id;
                    }
                }

                this.firstNameDictionary.Add(rec.FirstName, new List<FileCabinetRecord>() { record });
                this.lastNameDictionary.Add(rec.LastName, new List<FileCabinetRecord>() { record });
                this.dateOfBirthDictionary.Add(rec.DateOfBirth, new List<FileCabinetRecord>() { record });
                Console.WriteLine($"Record #{this.GetStat()} is created.");

                return record.Id;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// This method returns all the records.
        /// </summary>
        /// <returns>Array of whole the records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            foreach (var record in this.list)
            {
                result.Add(record);
            }

            return result.ToArray();
        }

        /// <summary>
        /// This method returns the number of records.
        /// </summary>
        /// <returns>The number of records.</returns>
        public int GetStat() => this.list.Count;

        /// <summary>
        /// This method edits the record.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="rec">Record.</param>
        public void EditRecord(int id, ParameterObject rec)
        {
            try
            {
                if (id > this.list.Count || id < 0)
                {
                    throw new ArgumentException("There is no such a record");
                }

                if (rec == null)
                {
                    throw new ArgumentException("The record is null");
                }

                if (string.IsNullOrWhiteSpace(rec.FirstName) || rec.FirstName.Length < 2 || rec.FirstName.Length > 60)
                {
                    throw new ArgumentException("Exception because of the incorrect first name format");
                }

                if (string.IsNullOrWhiteSpace(rec.LastName) || rec.LastName.Length < 2 || rec.LastName.Length > 60)
                {
                    throw new ArgumentException("Exception because of the incorrect last name format");
                }

                var record = new FileCabinetRecord
                {
                    Id = id,
                    FirstName = rec.FirstName,
                    LastName = rec.LastName,
                    DateOfBirth = rec.DateOfBirth,
                    Age = rec.Age,
                    FavouriteNumeral = rec.FavouriteNumeral,
                    Income = rec.Income,
                };

                this.list[id - 1] = record;

                foreach (var firstname in this.firstNameDictionary)
                {
                    if (rec.FirstName == firstname.Key)
                    {
                        firstname.Value.Clear();
                        firstname.Value.Add(record);
                        return;
                    }
                }

                foreach (var lastname in this.lastNameDictionary)
                {
                    if (rec.LastName == lastname.Key)
                    {
                        lastname.Value.Clear();
                        lastname.Value.Add(record);
                        return;
                    }
                }

                foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
                {
                    if (rec.DateOfBirth == dateOfBirthFromDictionary.Key)
                    {
                        dateOfBirthFromDictionary.Value.Clear();
                        dateOfBirthFromDictionary.Value.Add(record);
                        return;
                    }
                }

                this.lastNameDictionary.Add(rec.LastName, new List<FileCabinetRecord>() { record });
                this.firstNameDictionary.Add(rec.FirstName, new List<FileCabinetRecord>() { record });
                this.dateOfBirthDictionary.Add(rec.DateOfBirth, new List<FileCabinetRecord>() { record });
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#{id} record not found");
            }
        }

        /// <summary>
        /// This method is searching for all the reords with specified name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Array of the records with specified first names.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (var firstname in this.firstNameDictionary)
            {
                if (firstname.Key.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                {
                    return firstname.Value.ToArray();
                }
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// This method is searching for all the reords with specified last name.
        /// </summary>
        /// <param name="lastname">Last name.</param>
        /// <returns>Array of the records with specified last names.</returns>
        public FileCabinetRecord[] FindByLastName(string lastname)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (var lastName in this.lastNameDictionary)
            {
                if (lastName.Key.Equals(lastname, StringComparison.OrdinalIgnoreCase))
                {
                    return lastName.Value.ToArray();
                }
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// This method is searching for all the reords with specified date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Array of the records with specified date of birth.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
            {
                if (dateOfBirthFromDictionary.Key == dateOfBirth)
                {
                    return dateOfBirthFromDictionary.Value.ToArray();
                }
            }

            return Array.Empty<FileCabinetRecord>();
        }
    }
}
