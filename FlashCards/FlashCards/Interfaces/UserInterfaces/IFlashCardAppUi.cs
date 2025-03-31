namespace FlashCards
{
    /// <summary>
    /// Represents a user interface for the FlashCardApp.
    /// Implements IUserInterface
    /// </summary>
    internal interface IFlashCardAppUi : IUserInterface
    {
        /// <summary>
        /// Retrieves a valid FlashCardMenuOption based on user input
        /// </summary>
        /// <returns>A FlashCardMenuOption representing user choice</returns>
        FlashCardMenuOption GetFlashCardMenuSelection();
        /// <summary>
        /// Retrieves a valid MainMenuOption based on user input
        /// </summary>
        /// <returns>A MainMenuOption representing user choice</returns>
        MainMenuOption GetMainMenuSelection();
        /// <summary>
        /// Retrieves a valid StackMenuOption based on user input
        /// </summary>
        /// <returns>A StackMenuOption representing user choice</returns>
        StackMenuOption GetStackMenuSelection();
    }
}