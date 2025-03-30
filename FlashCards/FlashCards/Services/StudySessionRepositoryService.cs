using Spectre.Console;

namespace FlashCards
{
    internal class StudySessionRepositoryService
    {
        public StudySessionRepository StudySessionRepository { get; set; }

        public StudySessionRepositoryService(StudySessionRepository repository)
        {
            StudySessionRepository = repository;
        }
        public void PrepareRepository(List<CardStack> stacks, List<StudySession> defaultData)
        {

            if (!StudySessionRepository.DoesTableExist())
            {
                StudySessionRepository.CreateTable();
                StudySessionRepository.AutoFill(stacks, defaultData);
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

                PrintCard(stack.StackName, card.FrontText);
                input = GetAnswer();
                score += ValidateAnswer(card, input);
                numberOfRounds++;

                Console.WriteLine("\nPress any key to continue or ESC to exit");
                

            } while (Console.ReadKey().Key != ConsoleKey.Escape);

            StudySession session = new StudySession()
            {
                StackName = stack.StackName,
                StackId = stack.StackID,
                SessionDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day),
                Score = score,
            };

            PrintResult(session, numberOfRounds);
            InsertSession(session);

        }
        private void InsertSession(StudySession session) => StudySessionRepository.Insert(session);
        private void PrintCard(string stack, string frontText)
        {
            var table = new Table();
            table.AddColumn("Stack");
            table.AddColumn("Front Side");
            table.AddRow(stack, frontText);

            AnsiConsole.Write(table);
        }
        private string GetAnswer()
        {
            Console.Write("Enter your answer: ");
            string? input = Console.ReadLine();

            return input == null ? string.Empty : input;
        }
        public int ValidateAnswer(FlashCardDto card, string answer)
        {
            if (answer.ToLower() == card.BackText.ToLower())
            {
                Console.WriteLine("Correct!");
                return 1;
            }
            else
            {
                Console.WriteLine("Incorrect!");
                Console.WriteLine($"Your Answer: {answer}");
                Console.WriteLine($"Correct answer is: {card.BackText}");
                return 0;
            }
        }
        public void PrintResult(StudySession session, int numberOfRounds)
        {
            Console.Clear();
            Console.WriteLine("Study session results:");
            Console.WriteLine();

            var table = new Table();
            table.AddColumns("1", "2");
            table.HideHeaders();
            table.AddRow("Date", session.SessionDate.ToString("yyyy-MM-dd"));
            table.AddRow("Stack", session.StackName);
            table.AddRow("Rounds Played", numberOfRounds.ToString());
            table.AddRow("Score", session.Score.ToString());
            table.AddRow("Accuracy", (session.Score * 100 / numberOfRounds).ToString());

            AnsiConsole.Write(table);

            Console.WriteLine("\nPress any key to continue or ESC to exit");
            Console.ReadKey();
        }

        public void PrintAllSessions(List<CardStack> stacks)
        {
            var sessions = StudySessionRepository.GetAllRecords().ToList();

            var table = new Table();
            table.AddColumns("Date", "Details");

            foreach (var session in sessions) 
            {
                string stack = stacks.First(x => x.StackID == session.StackId).StackName;
                string sessionDate = session.SessionDate.ToString("yyyy-MM-dd");
                string sessionInfo = $"{stack}\nScore: {session.Score}";

                table.AddRow(sessionDate, sessionInfo);
            }
            table.ShowRowSeparators();
            AnsiConsole.Write(table);
            Console.WriteLine("\nPress any key to continue or ESC to exit");
            Console.ReadKey();
        }

        public void PrintReport(List<CardStack> stacks) 
        {

            int year = GetYear();

            foreach(var stack in stacks)
            {
                PrintReportForStack(stack, year);
            }
            Console.ReadLine();

        }
        private void PrintReportForStack(CardStack stack, int year)
        {
            ReportObject? sessionCount = StudySessionRepository.GetDataPerMonthInYear(stack,year, PivotFunction.Count);
            ReportObject? sessionScoreSumary = StudySessionRepository.GetDataPerMonthInYear(stack, year, PivotFunction.Sum);
            ReportObject? sessionScoreAvg = StudySessionRepository.GetDataPerMonthInYear(stack, year, PivotFunction.Average);

            if (sessionCount == null || sessionScoreSumary == null || sessionScoreAvg == null)
            {
                Console.WriteLine("No Data to be reported");
            }
            else
            {
                Table table = new Table();
                table.Title = new TableTitle($"Stack: {stack.StackName}, Year: {year}");
                table.AddColumns("", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
                table.AddRow(GetTableRow("Sessions", sessionCount));
                table.AddRow(GetTableRow("Total Score", sessionScoreSumary));
                table.AddRow(GetTableRow("Avg Score", sessionScoreAvg));
                table.ShowRowSeparators();

                AnsiConsole.Write(table);
            }

            
        }
        private string[] GetTableRow(string text, ReportObject data)
        {
            return new string[] { 
                text, 
                data.January.ToString(),
                data.February.ToString(),
                data.March.ToString(),
                data.April.ToString(),
                data.May.ToString(),
                data.June.ToString(),
                data.July.ToString(),
                data.August.ToString(),
                data.September.ToString(),
                data.November.ToString(),
                data.October.ToString(),
                data.December.ToString(),
            };
        }

        private int GetYear()
        {
            int thisYear = DateTime.Now.Year;
            var year = AnsiConsole.Prompt(
                new TextPrompt<int>("Select year for the report")
                .DefaultValue(2025)
                );
            return year;


        }
    }
}
