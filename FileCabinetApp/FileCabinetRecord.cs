using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Structure of the records.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets the Id number.
        /// </summary>
        /// <value>positive integer number.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>not null string.</value>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>not null string.</value>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>DateTime.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>Positive short number.</value>
        public short Age { get; set; }

        /// <summary>
        /// Gets or sets the favourite numeral.
        /// </summary>
        /// <value>char 1-9.</value>
        public char FavouriteNumeral { get; set; }

        /// <summary>
        /// Gets or sets the income.
        /// </summary>
        /// <value>decimal number.</value>
        public decimal Income { get; set; }
    }
}
