using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// A class for recording xml data.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Writes the data in the xml file.
        /// </summary>
        /// <param name="record">A record.</param>
        public void Write(FileCabinetRecord record)
        {
            if (this.writer is not null && record is not null)
            {
                this.writer.WriteStartElement("record");
                this.writer.WriteAttributeString("id", record.Id.ToString(new CultureInfo("En-en")));
                this.writer.WriteStartElement("name");
                this.writer.WriteAttributeString("first", record.FirstName);
                this.writer.WriteAttributeString("last", record.LastName);
                this.writer.WriteEndElement();
                this.writer.WriteElementString("dateOfBirth", record.DateOfBirth.ToString("dd/MM/yyyy", new CultureInfo("En-en")));
                this.writer.WriteElementString("age", record.Age.ToString(new CultureInfo("En-en")));
                this.writer.WriteElementString("favouriteNumeral", record.FavouriteNumeral.ToString(new CultureInfo("En-en")));
                this.writer.WriteElementString("income", record.Income.ToString(new CultureInfo("En-en")));
                this.writer.WriteEndElement();
            }
        }
    }
}
