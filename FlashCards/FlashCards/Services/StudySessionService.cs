namespace FlashCards
{
    internal class StudySessionService : IStudySessionService
    {
        public IStudySessionRepository StudySessionRepository { get; set; }
        public IStudySessionServiceUi UserInterface { get; set; }
        public StudySessionService(IStudySessionRepository repository, IStudySessionServiceUi UI)
        {
            StudySessionRepository = repository;
            UserInterface = UI;
        }

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
        private void InsertSession(StudySession session) => StudySessionRepository.Insert(session);

        public void PrintAllSessions(List<CardStack> stacks)
        {
            var sessions = StudySessionRepository.GetAllRecords()?.ToList()
                ?? new List<StudySession>();

            UserInterface.PrintAllSessions(stacks, sessions);
        }

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
