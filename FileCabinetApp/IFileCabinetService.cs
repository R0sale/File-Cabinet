namespace FileCabinetApp
{
    /// <summary>
    /// Interface for extract interface.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creating the record.
        /// </summary>
        /// <param name="rec">The record.</param>
        /// <returns>The id of the record.</returns>
        int CreateRecord(ParameterObject rec);

        /// <summary>
        /// This method edits the record.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="rec">Record.</param>
        /// <returns>Return the id of the record.</returns>
        int EditRecord(int id, ParameterObject rec);

        /// <summary>
        /// This method is searching for all the reords with specified date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Array of the records with specified date of birth.</returns>
        IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// This method is searching for all the reords with specified name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Array of the records with specified first names.</returns>
        IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// This method is searching for all the reords with specified last name.
        /// </summary>
        /// <param name="lastname">Last name.</param>
        /// <returns>Array of the records with specified last names.</returns>
        IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastname);

        /// <summary>
        /// This method returns all the records.
        /// </summary>
        /// <returns>Array of whole the records.</returns>
        IReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// This method returns the number of records.
        /// </summary>
        /// <returns>The number of records.</returns>
        // pragma because it is the initial code.
        int GetStat();

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>The snapshot.</returns>
        FileCabinetServiceSnapshot MakeSnapshot();

        void Restore(FileCabinetServiceSnapshot snapshot);
    }
}