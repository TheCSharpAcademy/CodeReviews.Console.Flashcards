namespace FlashCards
{
    /// <summary>
    /// Defines the contract for a repository managing StudySession entities.
    /// </summary>
    internal interface IStudySessionRepository
    {
        /// <summary>
        /// Property representing connection string used to connect to the database.
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// Checks if the StudySession table exists in the database.
        /// </summary>
        /// <returns>True if the table exists, otherwise false.</returns
        bool DoesTableExist();
        /// <summary>
        /// Creates the table for storing StudySession entities in the database.
        /// </summary>
        /// <returns>True if the table was created successfully, otherwise false.</return
        void CreateTable();
        /// <summary>
        /// Populates the repository with default data.
        /// </summary>
        /// <param name="stacks">A list of default CardStack entities</param>
        /// <param name="sessions">A list of default StudySession entities to insert</param>
        void AutoFill(List<CardStack> stacks, List<StudySession> sessions);
        /// <summary>
        /// Inserts a new StudySession entity into the database.
        /// </summary>
        /// <param name="entity">The StudySession entity to insert.</param>
        /// <returns>True if the insertion was successful, otherwise false.</returns>
        bool Insert(StudySession entity);
        /// <summary>
        /// Retrieves all FlashCard records from the database.
        /// </summary>
        /// <returns>An enumerable collection of CardStack entities.</returns>
        IEnumerable<StudySession>? GetAllRecords();

        /// <summary>
        /// Retrieves ReportObject entity based on passed parameters
        /// </summary>
        /// <param name="stack">A CardStack entity for which will data be retrieved</param>
        /// <param name="year">A Integer representing year for the record</param>
        /// <param name="pivotFunction">A Enumerable PivotFunction representing data to be retrieved</param>
        /// <returns>ReportObject entity based on passed parameters</returns>
        ReportObject? GetDataPerMonthInYear(CardStack stack, int year, PivotFunction pivotFunction);

       
    }
}