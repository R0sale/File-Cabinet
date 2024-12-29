using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for custom validation.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// Class is calling the base class constructor and send there Custom Validator object.
        /// </summary>
        public FileCabinetCustomService()
            : base(new CustomValidator())
        {
        }

        /// <summary>
        /// Creating validator by custom rules.
        /// </summary>
        /// <returns>Object of IRecordValidator interface.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
