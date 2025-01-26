using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// FileCabinetService class is maintaining the whole records and set methods to use the required behaviour.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private IRecordValidator? validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// constructor injection.
        /// </summary>
        /// <param name="validator">A validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
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

                List<int> indexes = new List<int>();
                if (this.list.Where(elem => elem.Id == 1).ToArray().Length != 0)
                {
                    for (int i = 0; i < this.list.Count; i++)
                    {
                        for (int j = 0; j < this.list.Count; j++)
                        {
                            if (this.list[i].Id + 1 == this.list[j].Id)
                            {
                                break;
                            }
                            else if (j == this.list.Count - 1)
                            {
                                indexes.Add(this.list[i].Id);
                            }
                        }
                    }
                }
                else
                {
                    indexes.Add(0);
                }

#pragma warning disable CA1062

                // all the pragma warnings are made, because the validation is in the ValidateParameters, so there's no need to worry about nullable rec.
                var record = new FileCabinetRecord
                {
                    Id = indexes.Min() + 1,
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
                        else if (firstname.Key.Equals(this.firstNameDictionary.Last().Key, StringComparison.Ordinal))
                        {
                            this.firstNameDictionary.Add(rec.FirstName, new List<FileCabinetRecord>() { record });
                            break;
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
                        else if (lastname.Key.Equals(this.lastNameDictionary.Last().Key, StringComparison.Ordinal))
                        {
                            this.lastNameDictionary.Add(rec.LastName, new List<FileCabinetRecord>() { record });
                            break;
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
                        else if (dateOfBirthFromDictionary.Key.Equals(this.dateOfBirthDictionary.Last().Key))
                        {
                            this.dateOfBirthDictionary.Add(rec.DateOfBirth, new List<FileCabinetRecord>() { record });
                            break;
                        }
                    }
                }
                else
                {
                    this.dateOfBirthDictionary.Add(rec.DateOfBirth, new List<FileCabinetRecord>() { record });
                }

#pragma warning restore CS8604
                Console.WriteLine($"Record #{indexes.Min() + 1} is created.");
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

            List<int> ids = new List<int>();

            foreach (var record in this.list)
            {
                ids.Add(record.Id);
            }

            ids.Sort();

            foreach (int id in ids)
            {
                for (int i = 0; i < this.list.Count; i++)
                {
                    if (id == this.list[i].Id)
                    {
                        result.Add(this.list[i]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This method returns the number of records.
        /// </summary>
        /// <returns>The number of records.</returns>
        // pragma because it is the initial code.
#pragma warning disable CA1024
        public int GetStat()
        {
            return this.list.Count;
        }
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
                    if (firstname.Value.Where(x => x.Id == id).ToArray().Length != 0)
                    {
                        firstname.Value.Remove(firstname.Value.Where(x => x.Id == id).ToArray()[0]);
                    }
                }

                foreach (var lastname in this.lastNameDictionary)
                {
                    if (lastname.Value.Where(x => x.Id == id).ToArray().Length != 0)
                    {
                        lastname.Value.Remove(lastname.Value.Where(x => x.Id == id).ToArray()[0]);
                    }
                }

                foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
                {
                    if (dateOfBirthFromDictionary.Value.Where(x => x.Id == id).ToArray().Length != 0)
                    {
                        dateOfBirthFromDictionary.Value.Remove(dateOfBirthFromDictionary.Value.Where(x => x.Id == id).ToArray()[0]);
                    }
                }

                foreach (var firstname in this.firstNameDictionary)
                {
                    if (rec.FirstName == firstname.Key)
                    {
                        firstname.Value.Add(record);
                        break;
                    }
                    else if (this.firstNameDictionary.Last().Equals(firstname))
                    {
                        this.firstNameDictionary.Add(rec.FirstName, new List<FileCabinetRecord>() { record });
                        break;
                    }
                }

                foreach (var lastname in this.lastNameDictionary)
                {
                    if (rec.LastName == lastname.Key)
                    {
                        lastname.Value.Add(record);
                        break;
                    }
                    else if (this.lastNameDictionary.Last().Equals(lastname))
                    {
                        this.lastNameDictionary.Add(rec.LastName, new List<FileCabinetRecord>() { record });
                        break;
                    }
                }

                foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
                {
                    if (rec.DateOfBirth == dateOfBirthFromDictionary.Key)
                    {
                        dateOfBirthFromDictionary.Value.Add(record);
                        break;
                    }
                    else if (this.dateOfBirthDictionary.Last().Equals(dateOfBirthFromDictionary))
                    {
                        this.dateOfBirthDictionary.Add(rec.DateOfBirth, new List<FileCabinetRecord>() { record });
                        break;
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

        /// <summary>
        /// The method for loading the records from the snapshot for the list and dictionaries.
        /// </summary>
        /// <param name="snapshot">A snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            try
            {
                if (snapshot is null)
                {
                    throw new ArgumentException("The snapshot is null");
                }

                if (snapshot.Records is null)
                {
                    throw new ArgumentException("The records are null");
                }

                foreach (var record in snapshot.Records)
                {
                    var existingRecord = this.list.FirstOrDefault(elem => elem.Id == record.Id);

                    if (existingRecord != null)
                    {
                        int index = this.list.IndexOf(existingRecord);
                        this.list[index] = record;
                    }
                    else
                    {
                        this.list.Add(record);
                    }

                    if (this.firstNameDictionary.Count != 0)
                    {
                        foreach (var firstname in this.firstNameDictionary)
                        {
                            if (record.FirstName == firstname.Key)
                            {
                                firstname.Value.Add(record);
                                break;
                            }
                            else if (firstname.Key.Equals(this.firstNameDictionary.Last().Key, StringComparison.Ordinal))
                            {
                                if (record.FirstName is null)
                                {
                                    throw new ArgumentException("The record is null");
                                }

                                this.firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord>() { record });
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (record.FirstName is null)
                        {
                            throw new ArgumentException("The record is null");
                        }

                        this.firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord>() { record });
                    }

                    if (this.lastNameDictionary.Count != 0)
                    {
                        foreach (var lastname in this.lastNameDictionary)
                        {
                            if (record.LastName == lastname.Key)
                            {
                                lastname.Value.Add(record);
                                break;
                            }
                            else if (lastname.Key.Equals(this.lastNameDictionary.Last().Key, StringComparison.Ordinal))
                            {
                                if (record.LastName is null)
                                {
                                    throw new ArgumentException("The record is null");
                                }

                                this.lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord>() { record });
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (record.LastName is null)
                        {
                            throw new ArgumentException("The record is null");
                        }

                        this.lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord>() { record });
                    }

                    if (this.dateOfBirthDictionary.Count != 0)
                    {
                        foreach (var dateOfBirthFromDictionary in this.dateOfBirthDictionary)
                        {
                            if (record.DateOfBirth == dateOfBirthFromDictionary.Key)
                            {
                                dateOfBirthFromDictionary.Value.Add(record);
                                break;
                            }
                            else if (dateOfBirthFromDictionary.Key.Equals(this.dateOfBirthDictionary.Last().Key))
                            {
                                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord>() { record });
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord>() { record });
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Removes records.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        public void RemoveRecords(int id)
        {
            try
            {
                int maxIndex = 0;
                int minIndex = 10000;
                foreach (var record in this.list)
                {
                    if (record.Id > maxIndex)
                    {
                        maxIndex = record.Id;
                    }
                }

                foreach (var record in this.list)
                {
                    if (record.Id < minIndex)
                    {
                        minIndex = record.Id;
                    }
                }

                if (id > maxIndex || id < minIndex)
                {
                    throw new ArgumentException($"The record {id} does not exist.");
                }

                foreach (var record in this.list)
                {
                    if (record.Id == id)
                    {
                        this.list.Remove(record);
                        break;
                    }
                }

                foreach (var firstName in this.firstNameDictionary)
                {
                    foreach (var record in firstName.Value)
                    {
                        if (record.Id == id)
                        {
                            firstName.Value.Remove(record);
                            if (firstName.Value.Count == 0)
                            {
                                this.firstNameDictionary.Remove(firstName.Key);
                            }

                            break;
                        }
                    }
                }

                foreach (var lastName in this.lastNameDictionary)
                {
                    foreach (var record in lastName.Value)
                    {
                        if (record.Id == id)
                        {
                            lastName.Value.Remove(record);
                            if (lastName.Value.Count == 0)
                            {
                                this.firstNameDictionary.Remove(lastName.Key);
                            }

                            break;
                        }
                    }
                }

                foreach (var datOfBirth in this.dateOfBirthDictionary)
                {
                    foreach (var record in datOfBirth.Value)
                    {
                        if (record.Id == id)
                        {
                            datOfBirth.Value.Remove(record);
                            if (datOfBirth.Value.Count == 0)
                            {
                                this.dateOfBirthDictionary.Remove(datOfBirth.Key);
                            }

                            break;
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// A method for FileCabinetFilesystemService.
        /// </summary>
        /// <exception cref="NotImplementedException">Not implemented in this class.</exception>
        public void PurgeRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns 0, because the number of deleted records for this instance is 0.
        /// </summary>
        /// <returns>0.</returns>
        public int GetDeletedRecords()
        {
            return 0;
        }
    }
}
