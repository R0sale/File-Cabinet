using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// A reader class.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private FileStream? fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="fileStream">FileStream.</param>
        public FileCabinetRecordXmlReader(FileStream? fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Deserializes the xml file.
        /// </summary>
        /// <returns>A collection of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Records), new XmlRootAttribute("records")
                {
                    Namespace = string.Empty,
                });

                using (this.fileStream)
                {
                    if (this.fileStream is null)
                    {
                        throw new ArgumentException("The filestream is null");
                    }

                    List<FileCabinetRecord> realRecords = new List<FileCabinetRecord>();
                    using (var reader = XmlReader.Create(this.fileStream))
                    {
                        Records? records = xmlSerializer.Deserialize(reader) as Records;

                        if (records is null)
                        {
                            throw new ArgumentException("The records are null");
                        }

                        if (records.RecordList is null)
                        {
                            throw new ArgumentException("The RecordList is null");
                        }

                        for (int i = 0; i < records.RecordList.Count; i++)
                        {
                            if (records.RecordList[i].RecordName is null)
                            {
                                throw new ArgumentException("The RecordList is null");
                            }

                            FileCabinetRecord record = new FileCabinetRecord()
                            {
                                Id = records.RecordList[i].Id,
                                FirstName = records.RecordList[i].RecordName?.FirstName,
                                LastName = records.RecordList[i].RecordName?.LastName,
                                DateOfBirth = records.RecordList[i].DateOfBirth,
                                Age = records.RecordList[i].Age,
                                FavouriteNumeral = records.RecordList[i].FavouriteNumeral,
                                Income = records.RecordList[i].Income,
                            };

                            realRecords.Add(record);
                        }

                        reader.Close();
                    }

                    return realRecords;
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return Array.Empty<FileCabinetRecord>();
            }
        }
    }
}
