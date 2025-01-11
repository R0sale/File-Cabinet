using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Write(FileCabinetRecord record)
        {
            if (record != null)
            {
                string csv = $"{record.FirstName},{record.LastName},{record.DateOfBirth},{record.Age},{record.FavouriteNumeral},{record.Income}";
                this.writer.WriteLine(csv);
            }
        }
    }
}
