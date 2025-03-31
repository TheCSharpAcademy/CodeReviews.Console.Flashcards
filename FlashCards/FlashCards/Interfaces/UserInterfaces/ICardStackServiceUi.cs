namespace FlashCards
{
    /// <summary>
    /// Represents a user interface for the ICardStackService.
    /// Implements IUserInterface
    /// </summary>
    internal interface ICardStackServiceUi : IUserInterface
    {
        /// <summary>
        /// Displays all CardStack entities from the list
        /// </summary>
        /// <param name="stacks">A List of CardStack entities to be displayed</param>
        void PrintStacks(List<CardStack> stacks);
    }
}