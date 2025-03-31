namespace FlashCards
{
    /// <summary>
    /// Defines the contract for a service managing StudySession entities.
    /// </summary>
    internal interface IStudySessionService
    {
        /// <summary>
        /// Gets the repository for interacting with StudySession entities.
        /// </summary>
        IStudySessionRepository StudySessionRepository { get; set; }

        /// <summary>
        /// Gets the user interface for interacting with StudySession entities.
        /// </summary>
        IStudySessionServiceUi UserInterface { get; set; }

        /// <summary>
        /// Handles runtime of the study session
        /// </summary>
        /// <param name="stack">A CardStack entity representing working stack</param>
        /// <param name="cards">A list of FlashCard entities if the stack</param>
        void NewStudySession(CardStack stack, List<FlashCardDto> cards);

        /// <summary>
        /// Prepares the repository by creating and populating it with default data if needed.
        /// </summary>
        /// <param name="stacks">A list of default CardStack entities.</param>
        /// <param name="defaultData">A list of default StudySession entities to insert.</param>
        /// <returns>True if the repository was successfully prepared, otherwise false.</returns>
        bool PrepareRepository(List<CardStack> stacks, List<StudySession> defaultData);

        /// <summary>
        /// Displays all Study Sessions for respective stack
        /// </summary>
        /// <param name="stacks">A CardStack entity for which records will be printed</param>
        void PrintAllSessions(List<CardStack> stacks);

        /// <summary>
        /// Prints report for StudySessions, ordered by CardStack
        /// </summary>
        /// <param name="stacks">A list of default CardStack entities.</param>
        void PrintReport(List<CardStack> stacks);
    }
}