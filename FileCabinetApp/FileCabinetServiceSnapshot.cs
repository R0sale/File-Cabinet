using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        public void SaveToCsv(StreamWriter writer)
        {
            FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(writer);

            PropertyInfo[] properties = new FileCabinetRecord().GetType().GetProperties();

            writer.WriteLine(string.Join(",", properties.Select(p => p.Name)));

            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }
    }
}
