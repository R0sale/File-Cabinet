using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// A snapshot class.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        public IReadOnlyCollection<FileCabinetRecord>? Records => Array.AsReadOnly(this.records);

        /// <summary>
        /// Saves the data to the csv file.
        /// </summary>
        /// <param name="writer">A writer for writing data.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(writer);

            PropertyInfo[] properties = new FileCabinetRecord().GetType().GetProperties();

            if (writer is not null)
            {
                writer.WriteLine(string.Join(",", properties.Select(p => p.Name)));

                foreach (var record in this.records)
                {
                    csvWriter.Write(record);
                }
            }
        }

        /// <summary>
        /// This is to save to the xml file.
        /// </summary>
        /// <param name="writer">A writer.</param>
        public void SaveToXml(StreamWriter writer)
        {
            using (XmlWriter xmlwriter = new XmlTextWriter(writer))
            {
                FileCabinetRecordXmlWriter xmlWriter = new FileCabinetRecordXmlWriter(xmlwriter);

                xmlwriter.WriteStartDocument();
                xmlwriter.WriteStartElement("records");

                foreach (var record in this.records)
                {
                    xmlWriter.Write(record);
                }

                xmlwriter.WriteEndElement();
                xmlwriter.WriteEndDocument();
            }
        }

        public void LoadFromCsv(StreamReader reader)
        {
            if (reader != null)
            {
                FileCabinetRecordCsvReader csvReader = new FileCabinetRecordCsvReader(reader);
                this.records = csvReader.ReadAll().ToArray();
            }
        }
    }
}
