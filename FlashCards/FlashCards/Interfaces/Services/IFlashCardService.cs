namespace FlashCards
{
    /// <summary>
    /// Defines the contract for a service managing FlashCard entities.
    /// </summary>
    internal interface IFlashCardService
    {
        /// <summary>
        /// Gets the repository for interacting with card stacks.
        /// </summary>
        IFlashCardRepository FlashCardRepository { get; set; }
        /// <summary>
        /// Gets the user interface for interacting with card stacks.
        /// </summary>
        IFlashCardServiceUi UserInterface { get; set; }

        /// <summary>
        /// Retrieves all FlashCard entities in a CardStack
        /// </summary>
        /// <param name="stack">A CardStack for which entities will be retrieved</param>
        /// <returns>A List of FlashCardDto object</returns>
        List<FlashCardDto>? GetAllCardsInStack(CardStack stack);
        /// <summary>
        /// Handles creation of new FlashCard
        /// </summary>
        /// <param name="stack">A CardStack for which FlashCard will be created</param>
        void HandleCreateNewFlashCard(CardStack stack);
        /// <summary>
        /// Handles deletion of a FlashCard
        /// </summary>
        /// <param name="stack">A CardStack for which FlashCard will be deleted</param>
        void HandleDeleteFlashCard(CardStack stack);
        /// <summary>
        /// Handles switchin of working stack
        /// </summary>
        /// <param name="stacks">A list of existing stacks</param>
        /// <returns>A CardStack entity representing selected Stack</returns>
        CardStack HandleSwitchStack(List<CardStack> stacks);
        /// <summary>
        /// Handles update of a FlashCard
        /// </summary>
        /// <param name="stack">A CardStack for which FlashCard will be updated</param>
        void HandleUpdateFlashCard(CardStack stack);
        /// <summary>
        /// Displays all available cards in a CardStack
        /// </summary>
        /// <param name="stack">A CardStack for which FlashCard will be displayed</param>
        void HandleViewAllCards(CardStack stack);
        /// <summary>
        /// Displays limited ammount of cards in a CardStack
        /// </summary>
        /// <param name="stack">A CardStack for which FlashCard will be displayed</param>
        void HandleViewXCards(CardStack stack);
        /// <summary>
        /// Prepares the repository by creating and populating it with default data if needed.
        /// </summary>
        /// <param name="stacks">A list of default CardStack entities.</param>
        /// <param name="defaultData">A list of default FlashCard entities to insert.</param>
        /// <returns>True if the repository was successfully prepared, otherwise false.</returns>
        bool PrepareRepository(List<CardStack> stacks, List<FlashCard> defaultData);
    }
}