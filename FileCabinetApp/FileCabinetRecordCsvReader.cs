using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// A reader class.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">A csv reder.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all the csv file.
        /// </summary>
        /// <returns>A collection of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.reader.ReadLine();
            while (!this.reader.EndOfStream)
            {
                string? line = this.reader.ReadLine();

                if (line != null)
                {
                    string[] values = line.Split(',');
                    try
                    {
                        FileCabinetRecord record = new FileCabinetRecord()
                        {
                            Id = int.Parse(values[0], new CultureInfo("En-en")),
                            FirstName = values[1],
                            LastName = values[2],
                            DateOfBirth = DateTime.ParseExact(values[3].Substring(0, 10), "dd.MM.yyyy", new CultureInfo("En-en")),
                            Age = short.Parse(values[4], new CultureInfo("En-en")),
                            FavouriteNumeral = char.Parse(values[5]),
                            Income = decimal.Parse(values[6], new CultureInfo("En-en")),
                        };

                        records.Add(record);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine($"{e.Message}. The invalid record is {values[0]}");
                    }
                }
            }

            return records;
        }
    }
}