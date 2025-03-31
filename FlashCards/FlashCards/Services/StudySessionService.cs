namespace FlashCards
{
    /// <summary>
    /// Represents a service for managing StudySession entities.
    /// Implements IStudySessionService
    /// </summary>
    internal class StudySessionService : IStudySessionService
    {
        /// <inheritdoc/>
        public IStudySessionRepository StudySessionRepository { get; set; }
        /// <inheritdoc/>
        public IStudySessionServiceUi UserInterface { get; set; }
        /// <summary>
        /// Intializes new object of FlashCardService class
        /// </summary>
        /// <param name="repository">A implementation of IStudySessionRepository for database access</param>
        /// <param name="UI">A implementation of IStudySessionServiceUi for user interaction</param>
        public StudySessionService(IStudySessionRepository repository, IStudySessionServiceUi UI)
        {
            StudySessionRepository = repository;
            UserInterface = UI;
        }
        /// <inheritdoc/>
        public bool PrepareRepository(List<CardStack> stacks, List<StudySession> defaultData)
        {
            try
            {
                if (!StudySessionRepository.DoesTableExist())
                {
                    StudySessionRepository.CreateTable();
                    StudySessionRepository.AutoFill(stacks, defaultData);
                }
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while preparing the repository");
                return false;
            }


        }
        /// <inheritdoc/>
        public void NewStudySession(CardStack stack, List<FlashCardDto> cards)
        {
            int numberOfRounds = 0;
            int score = 0;
            Random random = new Random();
            string input = string.Empty;

            do
            {
                Console.Clear();
                FlashCardDto card = cards[random.Next(cards.Count)];

                UserInterface.PrintQuestion(stack.StackName, card);
                input = UserInterface.GetStringFromUser("Your Answer: ");
                score += UserInterface.ValidateAnswer(card, input);
                numberOfRounds++;

            } while (UserInterface.GetKeyFromUser() != ConsoleKey.Escape);

            StudySession session = new StudySession()
            {
                StackName = stack.StackName,
                StackId = stack.StackID,
                SessionDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Score = score,
            };

            UserInterface.PrintResult(session, numberOfRounds);

            InsertSession(session);

        }
        /// <summary>
        /// Insert a new session to the Database
        /// </summary>
        /// <param name="session">A StudySession entity to be inserted to the database</param>
        private void InsertSession(StudySession session) => StudySessionRepository.Insert(session);

        /// <inheritdoc/>
        public void PrintAllSessions(List<CardStack> stacks)
        {
            var sessions = StudySessionRepository.GetAllRecords()?.ToList()
                ?? new List<StudySession>();

            UserInterface.PrintAllSessions(stacks, sessions);
        }
        /// <inheritdoc/>
        public void PrintReport(List<CardStack> stacks)
        {

            int year = UserInterface.GetNumberFromUser("Enter year for the report");

            foreach (var stack in stacks)
            {
                ReportObject? sessionCount = StudySessionRepository.GetDataPerMonthInYear(stack, year, PivotFunction.Count);
                ReportObject? sessionTotalScore = StudySessionRepository.GetDataPerMonthInYear(stack, year, PivotFunction.Sum);
                ReportObject? sessionScoreAvg = StudySessionRepository.GetDataPerMonthInYear(stack, year, PivotFunction.Average);

                if ((sessionCount == null || sessionTotalScore == null || sessionScoreAvg == null))
                { return; }

                UserInterface.PrintReportForStack(year, stack.StackName, sessionCount, sessionTotalScore, sessionScoreAvg);
            }
            Console.ReadLine();

        }
    }
}
