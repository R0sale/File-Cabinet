using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// FileCabinetService class is maintaining the whole records and set methods to use the required behaviour.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private IRecordValidator? validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// constructor injection.
        /// </summary>
        /// <param name="validator">A validator.</param>
        public FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// This method is creating the record.
        /// </summary>
        /// <param name="rec">Record.</param>
        /// <returns>Identify of the record.</returns>
        public int CreateRecord(ParameterObject rec)
        {
            try
            {
                if (this.validator == null)
                {
                    throw new ArgumentException("The validator is null");
                }

                this.validator.ValidateParameters(rec);

#pragma warning disable CA1062

                // all the pragma warnings are made, because the validation is in the ValidateParameters, so there's no need to worry about nullable rec.
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

#pragma warning restore CA1062

                this.list.Add(record);

                // pragma because everithing is validating in the validator
#pragma warning disable CS8604
                if (this.firstNameDictionary.Count != 0)
                {
                    foreach (var firstname in this.firstNameDictionary)
                    {
                        if (rec.FirstName == firstname.Key)
                        {
                            firstname.Value.Add(record);
                            break;
                        }
                        else if (firstname.Equals(this.firstNameDictionary.Last().Key))
                        {
                            this.firstNameDictionary.Add(rec.FirstName, new List<FileCabinetRecord>() { record });
                        }
                    }
                }
                else
                {
                    this.firstNameDictionary.Add(rec.FirstName, new List<FileCabinetRecord>() { record });
                }

                if (this.lastNameDictionary.Count != 0)
                {
                    foreach (var lastname in this.lastNameDictionary)
                    {
                        if (rec.LastName == lastname.Key)
                        {
                            lastname.Value.Add(record);
                            break;
                        }
                        else if (lastname.Equals(this.lastNameDictionary.Last().Key))
                        {
                            this.lastNameDictionary.Add(rec.LastName, new List<FileCabinetRecord>() { record });
                        }
                    }
                }
                else
                {
                    this.lastNameDictionary.Add(rec.LastName, new List<FileCabinetRecord>() { record });
                }

                if (this.dateOfBirthDictionary.Count != 0)
                {
                    foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
                    {
                        if (rec.DateOfBirth == dateOfBirthFromDictionary.Key)
                        {
                            dateOfBirthFromDictionary.Value.Add(record);
                            break;
                        }
                        else if (dateOfBirthFromDictionary.Equals(this.dateOfBirthDictionary.Last().Key))
                        {
                            this.dateOfBirthDictionary.Add(rec.DateOfBirth, new List<FileCabinetRecord>() { record });
                        }
                    }
                }
                else
                {
                    this.dateOfBirthDictionary.Add(rec.DateOfBirth, new List<FileCabinetRecord>() { record });
                }

#pragma warning restore CS8604
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
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            foreach (var record in this.list)
            {
                result.Add(record);
            }

            return result;
        }

        /// <summary>
        /// This method returns the number of records.
        /// </summary>
        /// <returns>The number of records.</returns>
        // pragma because it is the initial code.
#pragma warning disable CA1024
        public int GetStat() => this.list.Count;
#pragma warning restore CA1024
        /// <summary>
        /// This method edits the record.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="rec">Record.</param>
        /// <returns>Return the id of the record.</returns>
        public int EditRecord(int id, ParameterObject rec)
        {
            try
            {
                if (id > this.list.Count || id < 0)
                {
                    throw new ArgumentException($"#{id} record not found");
                }

                if (this.validator == null)
                {
                    throw new ArgumentException("The validator is null");
                }

                this.validator.ValidateParameters(rec);

#pragma warning disable CA1062

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
#pragma warning restore CA1062

                this.list[id - 1] = record;
#pragma warning disable CS8604

                foreach (var firstname in this.firstNameDictionary)
                {
                    if (rec.FirstName == firstname.Key)
                    {
                        if (firstname.Value.Where(x => x.Id == id).ToArray().Length != 0)
                        {
                            firstname.Value.Remove(firstname.Value.Where(x => x.Id == id).ToArray()[0]);
                        }

                        firstname.Value.Add(record);
                    }
                    else
                    {
                        this.lastNameDictionary.Add(rec.LastName, new List<FileCabinetRecord>() { record });
                    }
                }

                foreach (var lastname in this.lastNameDictionary)
                {
                    if (rec.LastName == lastname.Key)
                    {
                        if (lastname.Value.Where(x => x.Id == id).ToArray().Length != 0)
                        {
                            lastname.Value.Remove(lastname.Value.Where(x => x.Id == id).ToArray()[0]);
                        }

                        lastname.Value.Add(record);
                    }
                    else
                    {
                        this.firstNameDictionary.Add(rec.FirstName, new List<FileCabinetRecord>() { record });
                    }
                }

                foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
                {
                    if (rec.DateOfBirth == dateOfBirthFromDictionary.Key)
                    {
                        if (dateOfBirthFromDictionary.Value.Where(x => x.Id == id).ToArray().Length != 0)
                        {
                            dateOfBirthFromDictionary.Value.Remove(dateOfBirthFromDictionary.Value.Where(x => x.Id == id).ToArray()[0]);
                        }

                        dateOfBirthFromDictionary.Value.Add(record);
                    }
                    else
                    {
                        this.dateOfBirthDictionary.Add(rec.DateOfBirth, new List<FileCabinetRecord>() { record });
                    }
                }
#pragma warning restore CS8604
                return id;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// This method is searching for all the reords with specified name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Array of the records with specified first names.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
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
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastname)
        {
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
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
            {
                if (dateOfBirthFromDictionary.Key == dateOfBirth)
                {
                    return dateOfBirthFromDictionary.Value.ToArray();
                }
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Makes a snapshot of data.
        /// </summary>
        /// <returns>The snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }
    }
}
