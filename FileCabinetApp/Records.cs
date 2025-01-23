using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Records class for deserialization.
    /// </summary>
    [XmlRoot("records", Namespace ="")]
    public class Records
    {
#pragma warning disable
        /// <summary>
        /// Gets the list of the records.
        /// </summary>
        /// <value>A collection of records.</value>
        [XmlElement("record")]
        public Collection<ClassForDeserialization>? RecordList { get; set; }
#pragma warning enable
    }
}