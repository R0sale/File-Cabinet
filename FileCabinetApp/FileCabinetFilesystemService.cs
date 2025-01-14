using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;

        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

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

        public int EditRecord(int id, ParameterObject rec)
        {
            throw new NotImplementedException();
        }

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

        public int GetStat()
        {
            return (int)(this.fileStream.Length / 277);
        }

        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
