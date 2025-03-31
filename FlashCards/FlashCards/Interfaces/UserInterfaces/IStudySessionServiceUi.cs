namespace FlashCards
{
    /// <summary>
    /// Represents a user interface for the StudySessionService.
    /// Implements IUserInterface
    /// </summary>
    internal interface IStudySessionServiceUi : IUserInterface
    {
        /// <summary>
        /// Gets the date format to be used in the application.
        /// </summary>
        string DateFormat { get; }

        /// <summary>
        /// Prompts the user and gets a key press from the console.
        /// </summary>
        /// <returns>The key pressed by the user.</returns>
        ConsoleKey GetKeyFromUser();

        /// <summary>
        /// Prints the list of all study sessions along with their associated card stacks.
        /// </summary>
        /// <param name="stacks">The list of card stacks for the study session.</param>
        /// <param name="sessions">The list of study sessions.</param>
        void PrintAllSessions(List<CardStack> stacks, List<StudySession> sessions);

        /// <summary>
        /// Prints the question for the given flashcard, showing the stack name and card content.
        /// </summary>
        /// <param name="stack">The name of the card stack the card belongs to.</param>
        /// <param name="card">The flashcard to be displayed.</param>
        void PrintQuestion(string stack, FlashCardDto card);

        /// <summary>
        /// Prints a report for a specific stack, showing statistics such as session count, total score, and average score.
        /// </summary>
        /// <param name="year">The year for which the report is being generated.</param>
        /// <param name="stackName">The name of the stack.</param>
        /// <param name="sessionCount">The number of study sessions.</param>
        /// <param name="sessionTotalScore">The total score from the sessions.</param>
        /// <param name="sessionScoreAvg">The average score across sessions.</param>
        void PrintReportForStack(int year, string stackName, ReportObject sessionCount, ReportObject sessionTotalScore, ReportObject sessionScoreAvg);

        /// <summary>
        /// Prints the result of a study session after completing it, showing the session's performance.
        /// </summary>
        /// <param name="session">The study session to display the results for.</param>
        /// <param name="numberOfRounds">The number of rounds in the session.</param>
        void PrintResult(StudySession session, int numberOfRounds);

        /// <summary>
        /// Validates the user's answer for a given flashcard, checking if the answer is correct.
        /// </summary>
        /// <param name="card">The flashcard for which the answer is being validated.</param>
        /// <param name="answer">The user's answer to the card's question.</param>
        /// <returns>A numerical score (e.g., 0 for incorrect, 1 for correct).</returns>
        int ValidateAnswer(FlashCardDto card, string answer);
    }
}