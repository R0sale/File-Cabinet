using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// A class for filing system.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">FileStream.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Is creating the record.
        /// </summary>
        /// <param name="rec">The record object.</param>
        /// <returns>An integer of status.</returns>
        public int CreateRecord(ParameterObject rec)
        {
            if (rec is not null)
            {
                this.fileStream.Write(BitConverter.GetBytes((short)0), 0, 2);

                this.fileStream.Write(BitConverter.GetBytes((this.fileStream.Length / 277) + 1), 0, 4);
                ComplementaryFunctions.WriteFixedString(this.fileStream, rec.FirstName, 120);
                ComplementaryFunctions.WriteFixedString(this.fileStream, rec.LastName, 120);
                this.fileStream.Write(BitConverter.GetBytes(rec.DateOfBirth.Year), 0, 4);
                this.fileStream.Write(BitConverter.GetBytes(rec.DateOfBirth.Month), 0, 4);
                this.fileStream.Write(BitConverter.GetBytes(rec.DateOfBirth.Day), 0, 4);
                this.fileStream.Write(BitConverter.GetBytes(rec.Age), 0, 2);
                this.fileStream.Write(BitConverter.GetBytes(rec.FavouriteNumeral), 0, 1);
                ComplementaryFunctions.WriteFixedDecimal(this.fileStream, rec.Income, 16);
                this.fileStream.Flush();

                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Editting the record.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="rec">The record object.</param>
        /// <returns>An integer status.</returns>
        public int EditRecord(int id, ParameterObject rec)
        {
            try
            {
                List<FileCabinetRecord> records = this.GetRecords().ToList();

                foreach (FileCabinetRecord record in records)
                {
                    if (id == record.Id)
                    {
                        if (rec is null)
                        {
                            throw new ArgumentException("The record is null");
                        }

                        this.fileStream.Seek((id - 1) * 277, SeekOrigin.Begin);

                        this.fileStream.Write(BitConverter.GetBytes((short)0), 0, 2);

                        this.fileStream.Write(BitConverter.GetBytes(id), 0, 4);
                        ComplementaryFunctions.WriteFixedString(this.fileStream, rec.FirstName, 120);
                        ComplementaryFunctions.WriteFixedString(this.fileStream, rec.LastName, 120);
                        this.fileStream.Write(BitConverter.GetBytes(rec.DateOfBirth.Year), 0, 4);
                        this.fileStream.Write(BitConverter.GetBytes(rec.DateOfBirth.Month), 0, 4);
                        this.fileStream.Write(BitConverter.GetBytes(rec.DateOfBirth.Day), 0, 4);
                        this.fileStream.Write(BitConverter.GetBytes(rec.Age), 0, 2);
                        this.fileStream.Write(BitConverter.GetBytes(rec.FavouriteNumeral), 0, 1);
                        ComplementaryFunctions.WriteFixedDecimal(this.fileStream, rec.Income, 16);
                        this.fileStream.Flush();

                        return 1;
                    }
                }

                return 0;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <returns>The collection of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            this.fileStream.Seek(0, SeekOrigin.Begin);

            const int recordSize = 277;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                byte[] buffer = new byte[recordSize];

                this.fileStream.Read(buffer, 0, recordSize);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(buffer, 0, recordSize);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    using (BinaryReader reader = new BinaryReader(memoryStream))
                    {
                        short status = reader.ReadInt16();

                        if (status != 0)
                        {
                            continue;
                        }

                        int id = reader.ReadInt32();
                        string firstName = ComplementaryFunctions.ReadFixedString(reader, 120);
                        string lastName = ComplementaryFunctions.ReadFixedString(reader, 120);
                        int year = reader.ReadInt32();
                        int month = reader.ReadInt32();
                        int day = reader.ReadInt32();
                        DateTime dateOfBirth = new DateTime(year, month, day);
                        short age = reader.ReadInt16();
                        char favouriteNumeral = reader.ReadChar();
                        decimal income = reader.ReadDecimal();

                        FileCabinetRecord record = new FileCabinetRecord()
                        {
                            Id = id,
                            FirstName = firstName,
                            LastName = lastName,
                            DateOfBirth = dateOfBirth,
                            Age = age,
                            FavouriteNumeral = favouriteNumeral,
                            Income = income,
                        };

                        records.Add(record);
                    }
                }
            }

            return records;
        }

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <returns>The number of records.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / 277);
        }

        /// <summary>
        /// Finds the record by firstname.
        /// </summary>
        /// <param name="firstName">Firstname.</param>
        /// <returns>Collection of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            try
            {
                List<FileCabinetRecord> records = this.GetRecords().ToList();
                List<FileCabinetRecord> result = new List<FileCabinetRecord>();

                foreach (FileCabinetRecord record in records)
                {
                    if (record.FirstName is null)
                    {
                        throw new ArgumentException("The firstname is null");
                    }

                    if (record.FirstName.Equals(firstName, StringComparison.Ordinal))
                    {
                        result.Add(record);
                    }
                }

                return result;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Gets the records by last name.
        /// </summary>
        /// <param name="lastname">Lastname.</param>
        /// <returns>The collection of the specified records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastname)
        {
            try
            {
                List<FileCabinetRecord> records = this.GetRecords().ToList();
                List<FileCabinetRecord> result = new List<FileCabinetRecord>();

                foreach (FileCabinetRecord record in records)
                {
                    if (record.LastName is null)
                    {
                        throw new ArgumentException("The lastName is null");
                    }

                    if (record.LastName.Equals(lastname, StringComparison.Ordinal))
                    {
                        result.Add(record);
                    }
                }

                return result;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Gets the records by dateofbirth.
        /// </summary>
        /// <param name="dateOfBirth">DateOfBirth.</param>
        /// <returns>The collection of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> records = this.GetRecords().ToList();
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            foreach (FileCabinetRecord record in records)
            {
                if (record.DateOfBirth.Equals(dateOfBirth))
                {
                    result.Add(record);
                }
            }

            return result;
        }

        /// <summary>
        /// Not implemeted in this class.
        /// </summary>
        /// <returns>An exception.</returns>
        /// <exception cref="NotImplementedException">Exception.</exception>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the elements from the snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            try
            {
                const int recordSize = 277;

                if (snapshot is null)
                {
                    throw new ArgumentException("The snapshot is null");
                }

                if (snapshot.Records is null)
                {
                    throw new ArgumentException("The records are null");
                }

                List<FileCabinetRecord> newRecords = snapshot.Records.ToList();

                List<FileCabinetRecord> initialRecords = this.GetRecords().ToList();

                foreach (var record in newRecords)
                {
                    var existingRecord = initialRecords.FirstOrDefault(elem => elem.Id == record.Id);

                    if (existingRecord != null)
                    {
                        int index = initialRecords.IndexOf(existingRecord);
                        initialRecords[index] = record;
                    }
                    else
                    {
                        initialRecords.Add(record);
                    }
                }

                for (int i = 0; i < initialRecords.Count; i++)
                {
                    this.fileStream.Seek(i * recordSize, SeekOrigin.Begin);

                    this.fileStream.Write(BitConverter.GetBytes((short)0), 0, 2);
                    this.fileStream.Write(BitConverter.GetBytes(initialRecords[i].Id), 0, 4);
                    ComplementaryFunctions.WriteFixedString(this.fileStream, initialRecords[i].FirstName, 120);
                    ComplementaryFunctions.WriteFixedString(this.fileStream, initialRecords[i].LastName, 120);
                    this.fileStream.Write(BitConverter.GetBytes(initialRecords[i].DateOfBirth.Year), 0, 4);
                    this.fileStream.Write(BitConverter.GetBytes(initialRecords[i].DateOfBirth.Month), 0, 4);
                    this.fileStream.Write(BitConverter.GetBytes(initialRecords[i].DateOfBirth.Day), 0, 4);
                    this.fileStream.Write(BitConverter.GetBytes(initialRecords[i].Age), 0, 2);
                    this.fileStream.Write(BitConverter.GetBytes(initialRecords[i].FavouriteNumeral), 0, 1);
                    ComplementaryFunctions.WriteFixedDecimal(this.fileStream, initialRecords[i].Income, 16);
                    this.fileStream.Flush();
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
