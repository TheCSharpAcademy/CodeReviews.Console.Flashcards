using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace FlashCards
{
    /// <summary>
    /// Represents a user interface for StudySessionService.
    /// Inherits from UserInterace
    /// Implements IStudySessionServiceUi
    /// </summary>
    internal class StudySessionServiceUi : UserInterface, IStudySessionServiceUi
    {
        /// <inheritdoc/>
        public string DateFormat { get; } = "yyyy-MM-dd";
        /// <inheritdoc/>
        public ConsoleKey GetKeyFromUser()
        {
            Console.WriteLine("\nPress any key to continue or ESC to exit");
            return Console.ReadKey().Key;
        }
        /// <inheritdoc/>
        public void PrintQuestion(string stack, FlashCardDto card)
        {
            var table = new Table();
            table.AddColumn("Stack");
            table.AddColumn("Front Side");
            table.AddRow(stack, card.FrontText);

            AnsiConsole.Write(table);
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public void PrintResult(StudySession session, int numberOfRounds)
        {
            Console.Clear();
            Console.WriteLine("Study session results:");
            Console.WriteLine();

            var table = new Table();
            table.AddColumns("1", "2");
            table.HideHeaders();
            table.AddRow("Date", session.SessionDate.ToString(DateFormat));
            table.AddRow("Stack", session.StackName);
            table.AddRow("Rounds Played", numberOfRounds.ToString());
            table.AddRow("Score", session.Score.ToString());
            table.AddRow("Accuracy", (session.Score * 100 / numberOfRounds).ToString() + " %");

            AnsiConsole.Write(table);

            Console.WriteLine("\nPress any key to continue or ESC to exit");
            Console.ReadKey();
        }
        /// <inheritdoc/>
        public void PrintAllSessions(List<CardStack> stacks, List<StudySession> sessions)
        {
            var table = new Table();
            table.ShowRowSeparators();
            table.AddColumns("Date", "Details");

            if (sessions.IsNullOrEmpty())
            {
                table.AddRow("No Data", "No Data");
            }
            else
            {
                Dictionary<int, string> stackIdToNameMap = stacks.Select(x => x).ToDictionary(x => x.StackID, x => x.StackName);

                foreach (var session in sessions)
                {
                    string stack = stackIdToNameMap[session.StackId];
                    string sessionDate = session.SessionDate.ToString(DateFormat);
                    string sessionInfo = $"{stack}\nScore: {session.Score}";

                    table.AddRow(sessionDate, sessionInfo);
                }
            }

            AnsiConsole.Write(table);
            Console.WriteLine("\nPress any key to continue or ESC to exit");
            Console.ReadKey();
        }
        /// <inheritdoc/>
        public void PrintReportForStack(int year, string stackName, ReportObject sessionCount, ReportObject sessionTotalScore, ReportObject sessionScoreAvg)
        {
            Table table = new Table();
            table.Title = new TableTitle($"Stack: {stackName}, Year: {year}");

            table.AddColumns("", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
            table.AddRow(GetTableRow("Sessions", sessionCount));
            table.AddRow(GetTableRow("Total Score", sessionTotalScore));
            table.AddRow(GetTableRow("Avg Score", sessionScoreAvg));
            table.ShowRowSeparators();

            AnsiConsole.Write(table);
        }
        /// <summary>
        /// Returns array of string values representing row in a Table
        /// </summary>
        /// <param name="text">A string value representing description of the data in ReportObject</param>
        /// <param name="data">A ReportObject containing the data</param>
        /// <returns>A string array</returns>
        private string[] GetTableRow(string text, ReportObject data)
        {
            return [
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
            ];
        }

    }
}
