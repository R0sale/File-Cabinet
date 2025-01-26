using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface for class strategies.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Method for validating parameters.
        /// </summary>
        /// <param name="rec">Object with the record.</param>
        void ValidateParameters(ParameterObject rec);
    }
}
