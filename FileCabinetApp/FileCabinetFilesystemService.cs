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

                this.fileStream.Write(BitConverter.GetBytes(rec.Id), 0, 4);
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
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
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
