namespace FlashCards
{
    /// <summary>
    /// Represents a user interface for the FlashCardServiceUi.
    /// Implements IUserInterface
    /// </summary>
    internal interface IFlashCardServiceUi : IUserInterface
    {
        /// <summary>
        /// Gets ID of the card from user. Available Cards are passed in the list
        /// </summary>
        /// <param name="cards">A List of Cards from which user have to choose</param>
        /// <returns>A integer representing card in the list</returns>
        int GetCardID(List<FlashCardDto> cards);

        /// <summary>
        /// Gets card information from the user
        /// </summary>
        /// <returns>A new FlashCard entity based on user input</returns>
        FlashCard GetNewCard();

        /// <summary>
        /// Displays all cards
        /// </summary>
        /// <param name="cards">A List of Cards to be displayed</param>
        void PrintCards(List<FlashCardDto> cards);
    }
}