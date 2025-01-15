using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    public class FileCabinetGeneratorRecord
    {
        /// <summary>
        /// Gets or sets the Id number.
        /// </summary>
        /// <value>positive integer number.</value>
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public Name? RecordName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>DateTime.</value>
        [XmlElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>Positive short number.</value>
        [XmlElement("age")]
        public short Age { get; set; }

        /// <summary>
        /// Gets or sets the favourite numeral.
        /// </summary>
        /// <value>char 1-9.</value>
        [XmlElement("favouriteNumeral")]
        public char FavouriteNumeral { get; set; }

        /// <summary>
        /// Gets or sets the income.
        /// </summary>
        /// <value>decimal number.</value>
        [XmlElement("income")]
        public decimal Income { get; set; }
    }

    public class Name
    {
        [XmlAttribute("first")]
        public string? FirstName { get; set; }
        [XmlAttribute("last")]
        public string? LastName { get; set; }
    }

    [XmlRoot("records")]
    public class Records
    {
        [XmlElement("record")]
        public List<FileCabinetGeneratorRecord>? RecordList { get; set; }
    }
}
