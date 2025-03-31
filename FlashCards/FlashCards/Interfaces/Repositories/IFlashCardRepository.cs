namespace FlashCards
{
    /// <summary>
    /// Defines the contract for a repository managing FlashCards entities.
    /// </summary>
    internal interface IFlashCardRepository
    {
        /// <summary>
        /// Property representing connection string used to connect to the database.
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// Creates the table for storing FlashCards entities in the database.
        /// </summary>
        /// <returns>True if the table was created successfully, otherwise false.</returns>
        bool DoesTableExist();
        /// <summary>
        /// Creates the table for storing FlashCards entities in the database.
        /// </summary>
        /// <returns>True if the table was created successfully, otherwise false.</returns>
        bool CreateTable();
        /// <summary>
        /// Populates the repository with default data.
        /// </summary>
        /// <param name="stacks">A list of default CardStack entities.</param>
        /// <param name="flashCards">A list of default FlashCard entities to insert.</param>

        void AutoFill(List<CardStack> stacks, List<FlashCard> flashCards);
        /// <summary>
        /// Inserts a new FlashCard entity into the database.
        /// </summary>
        /// <param name="entity">The FlashCard entity to insert.</param>
        /// <returns>True if the insertion was successful, otherwise false.</returns>
        bool Insert(FlashCard entity);
        /// <summary>
        /// Updates an existing FlashCard entity in the database.
        /// </summary>
        /// <param name="entity">The FlashCard entity to update.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        bool Update(FlashCard entity);
        /// <summary>
        /// Deletes a specified FlashCard entity from the database.
        /// </summary>
        /// <param name="entity">The FlashCard entity to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        bool Delete(FlashCard entity);

        /// <summary>
        /// Retrieves all FlashCard records from the database.
        /// </summary>
        /// <returns>An enumerable collection of CardStack entities.</returns>
        IEnumerable<FlashCard>? GetAllRecords();

        /// <summary>
        /// Retrieves all FlashCard records from the database for single stack.
        /// </summary>
        /// <param name="stack">The CardStack entity for which data will be retrieved</param>
        /// <returns>An enumerable collection of CardStack entities.</returns>
        IEnumerable<FlashCardDto>? GetAllRecordsFromStack(CardStack stack);
        /// <summary>
        /// Retrieves specified ammount of FlashCard records from the database for single stack.
        /// </summary>
        /// <param name="stack">The CardStack entity for which data will be retrieved</param>
        /// <param name="count">Number of records to be retrieved</param>
        /// <returns>An enumerable collection of CardStack entities.</returns>
        IEnumerable<FlashCardDto>? GetXRecordsFromStack(CardStack stack, int count);
        
    }
}