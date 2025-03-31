namespace FlashCards
{
    /// <summary>
    /// Represents a general user interface.
    /// Defines basic set of methods for user interaction and input handling.
    /// </summary>
    internal interface IUserInterface
    {
        /// <summary>
        /// Clears the console screen.
        /// </summary>
        void ClearConsole();

        /// <summary>
        /// Prompts the user with a message and retrieves an integer input.
        /// </summary>
        /// <param name="prompt">The message to display to the user.</param>
        /// <returns>The integer input from the user.</returns>
        int GetNumberFromUser(string prompt);

        /// <summary>
        /// Prompts the user with a message and retrieves a string input.
        /// </summary>
        /// <param name="prompt">The message to display to the user.</param>
        /// <returns>The string input from the user.</returns>
        string GetStringFromUser(string prompt);

        /// <summary>
        /// Displays the application header.
        /// </summary>
        void PrintApplicationHeader();

        /// <summary>
        /// Displays a message instructing the user to press any key to continue.
        /// </summary>
        void PrintPressAnyKeyToContinue();

        /// <summary>
        /// Allows the user to select a stack from a list of card stacks.
        /// </summary>
        /// <param name="stacks">A list of available card stacks.</param>
        /// <returns>The selected card stack.</returns>
        CardStack StackSelection(List<CardStack> stacks);
    }
}