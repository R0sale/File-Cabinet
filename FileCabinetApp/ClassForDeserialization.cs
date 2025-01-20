using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for deserialization with tags and attributes for Xml.
    /// </summary>
    public class ClassForDeserialization
    {
        /// <summary>
        /// Gets or sets the Id number.
        /// </summary>
        /// <value>positive integer number.</value>
        [XmlAttribute("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets record name.
        /// </summary>
        /// <value>Name that contains 2 strings: firstname and lastname.</value>
        [XmlElement("name")]
        public Name? RecordName { get; set; }

        /// <summary>
        /// Gets or sets the dateBirth time.
        /// </summary>
        /// <value>DateTime.</value>
        [XmlIgnore]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>DateTime.</value>
        [XmlElement("dateOfBirth")]
        public string DateOfBirthString
        {
            get => this.DateOfBirth.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("En-en"));
            set => this.DateOfBirth = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", null);
        }

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
}