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
    /// A name class.
    /// </summary>
    public class Name
    {
        /// <summary>
        /// Gets or sets firstname.
        /// </summary>
        /// <value>FirstName.</value>
        [XmlAttribute("first")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets lastname.
        /// </summary>
        /// <value>LastName.</value>
        [XmlAttribute("last")]
        public string? LastName { get; set; }
    }
}