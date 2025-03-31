namespace FlashCards
{
    /// <summary>
    /// Defines the contract for a service managing CardStack entities.
    /// </summary>
    internal interface ICardStackService
    {
        /// <summary>
        /// Gets the repository for interacting with card stacks.
        /// </summary>
        ICardStackRepository CardStackRepository { get; }

        /// <summary>
        /// Gets the user interface for interacting with card stacks.
        /// </summary>
        ICardStackServiceUi UserInterface { get; }

        /// <summary>
        /// Retrieves all existing card stacks.
        /// </summary>
        /// <returns>A list of <see cref="CardStack"/> objects.</returns>
        List<CardStack> GetAllStacks();

        /// <summary>
        /// Handles the creation of a new card stack based on user input.
        /// </summary>
        void HandleCreateNewStack();

        /// <summary>
        /// Handles the deletion of a selected card stack.
        /// </summary>
        void HandleDeleteStack();

        /// <summary>
        /// Handles renaming a selected card stack.
        /// </summary>
        void HandleRenameStack();

        /// <summary>
        /// Displays all available card stacks.
        /// </summary>
        void HandleViewAllStacks();

        /// <summary>
        /// Prepares the repository by creating and populating it with default data if needed.
        /// </summary>
        /// <param name="defaultData">The list of default card stacks.</param>
        /// <returns>True if the repository was successfully prepared, otherwise false.</returns>
        bool PrepareRepository(List<CardStack> defaultData);
    }
}