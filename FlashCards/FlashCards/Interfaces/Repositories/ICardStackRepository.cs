namespace FlashCards
{
    /// <summary>
    /// Defines the contract for a repository managing CardStack entities.
    /// </summary>
    internal interface ICardStackRepository
    {
        /// <summary>
        /// Property representing connection string used to connect to the database.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Checks if the CardStack table exists in the database.
        /// </summary>
        /// <returns>True if the table exists, otherwise false.</returns>
        bool DoesTableExist();

        /// <summary>
        /// Creates the table for storing CardStack entities in the database.
        /// </summary>
        /// <returns>True if the table was created successfully, otherwise false.</returns>
        bool CreateTable();

        /// <summary>
        /// Populates the repository with default data.
        /// </summary>
        /// <param name="defaultData">A list of default CardStack entities to insert.</param>
        void AutoFill(List<CardStack> defaultData);

        /// <summary>
        /// Inserts a new CardStack entity into the database.
        /// </summary>
        /// <param name="entity">The CardStack entity to insert.</param>
        /// <returns>True if the insertion was successful, otherwise false.</returns>
        bool Insert(CardStack entity);

        /// <summary>
        /// Updates an existing CardStack entity in the database.
        /// </summary>
        /// <param name="entity">The CardStack entity to update.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        bool Update(CardStack entity);

        /// <summary>
        /// Deletes a specified CardStack entity from the database.
        /// </summary>
        /// <param name="entity">The CardStack entity to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        bool Delete(CardStack entity);

        /// <summary>
        /// Retrieves all CardStack records from the database.
        /// </summary>
        /// <returns>An enumerable collection of CardStack entities.</returns>
        IEnumerable<CardStack> GetAllRecords();
    }
}