using System.Globalization;
using Flashcards.UndercoverDev.Extensions;
using Flashcards.UndercoverDev.Models;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.Repository.StudySessions;
using Flashcards.UndercoverDev.UserInteraction;
using Spectre.Console;

namespace Flashcards.UndercoverDev.Services.Session
{
    public class SessionServices : ISessionServices
    {
        private readonly IUserConsole _userConsole;
        private readonly ISessionRepository _sessionRepository;
        private readonly IStackRepository _stackRepository;
        private readonly IFlashcardRepository _flashcardRepository;

        public SessionServices(IUserConsole userConsole, ISessionRepository sessionRepository, IStackRepository stackRepository, IFlashcardRepository flashcardRepository)
        {
            _userConsole = userConsole;
            _sessionRepository = sessionRepository;
            _stackRepository = stackRepository;
            _flashcardRepository = flashcardRepository;
        }

        public void StartSession()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.ShowMenu("[bold]Select a [blue]Stack[/] to study[/]", stackName);

            if (selectedStackName == "Back")
                return;

            var retrievedStack = _stackRepository.GetStackByName(selectedStackName);
            var flashcards = _flashcardRepository.GetFlashcardsByStackId(retrievedStack.Id);

            if (flashcards.Count == 0)
            {
                _userConsole.PrintMessage("[bold]There are no flashcards in this stack.[/] [white]Press any key to continue.[/]", "red");
                _userConsole.WaitForAnyKey();
                return;
            }

            int score = 0;
            int index = 1;
            
            foreach (var flashcard in flashcards)
            {
                var table = CreateTable(index,"[orange1]FlashcardId[/]", "[lime]Question[/]", flashcard.Question);

                _userConsole.WritTable(table);

                string userAnswer;
                do
                {
                    userAnswer = _userConsole.GetUserInput("\nPlease enter your [green]answer[/] to the above flashcard: ");
                }
                while (string.IsNullOrEmpty(userAnswer));

                if (userAnswer.TrimAndLower() == flashcard.Answer.TrimAndLower())
                {
                    table = CreateTable(index,"[orange1]FlashcardId[/]", "[lime]Answer[/]", flashcard.Answer);
                    _userConsole.WritTable(table);
                    score++;
                    _userConsole.PrintMessage($"[bold]Correct! Your current score is [green]{score}[/]. Press any key to continue.[/]", "");
                }
                else
                {
                    table = CreateTable(index,"[orange1]FlashcardId[/]", "[lime]Answer[/]", flashcard.Answer);
                    _userConsole.WritTable(table);
                    _userConsole.PrintMessage("[bold]Incorrect! [white]Press any key to continue.[/][/]", "red");
                }
                _userConsole.WaitForAnyKey();
            }
            _userConsole.PrintMessage($"[bold]Study session completed. Your final score: [green]{score}/{flashcards.Count}[/]. Press any key to continue.[/]", "");
            _userConsole.WaitForAnyKey();

            _sessionRepository.Post(retrievedStack.Id, score, flashcards.Count);
            _userConsole.PrintMessage("[bold]1 [green]study session added[/]. Press any key to continue.[/]", "");
            _userConsole.WaitForAnyKey();
        }

        public void ViewSession()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.ShowMenu("[bold]Select a [blue]Stack[/] to view study sessions[/]", stackName);

            if (selectedStackName == "Back")
                return;

            var retrievedStack = _stackRepository.GetStackByName(selectedStackName);
            var sessions = _sessionRepository.GetSessionsByStackId(retrievedStack.Id);

            if (sessions.Count == 0)
            {
                _userConsole.PrintMessage("[bold]There are no study sessions in this stack. [white]Press any key to continue.[/][/]", "red");
                _userConsole.WaitForAnyKey();
                return;
            }

            _userConsole.PrintMessage($"{selectedStackName} [blue]Study Sessions[/]", "green");
            var table = new Table() {Border = TableBorder.Double};
            table.AddColumn("[orange1]Session Date[/]");
            table.AddColumn("[green]Score[/]");

            int averageScore = 0;
            foreach (var session in sessions)
            {
                averageScore += session.Score;
                table.AddRow($"[lime]{session.SessionDate}[/]", $"[lime]{session.Score}/{session.TotalQuestions}[/]");
            }
            averageScore /= sessions.Count;

            _userConsole.WritTable(table);
            _userConsole.PrintMessage($"[bold][skyblue1]Average study session score: {averageScore}[/][/]", "");
            _userConsole.PrintMessage("[bold][blue]Press any key to continue.[/][/]", "");
            _userConsole.WaitForAnyKey();
        }

        public void DeleteSession(int stackId)
        {
            var sessions = _sessionRepository.GetSessionsByStackId(stackId);

            foreach (var session in sessions)
            {
                _sessionRepository.Delete(session);
            }
        }

        // Helper functions
        public Table CreateTable(int index, string column1, string column2, string flashcardString)
        {
            var table = new Table() {Border = TableBorder.Double};
            table.AddColumn(new TableColumn(column1).RightAligned());
            table.AddColumn(column2);
            table.AddRow($"[lime]{index++}[/]", $"[lime]{flashcardString}[/]");

            return table;
        }

        public List<YearlyStudySessionReport> GenerateYearlyReport()
        {
            // var date = _userConsole.GetUserInput("Please enter a year in [darkgreen](Format: yyyy)[/]");

            var validDate = CheckIfYearIsValid();

            var studySessions = _sessionRepository.GetSessionsByYear(validDate);

            if (studySessions.Count == 0)
            {
                _userConsole.PrintMessage("[bold]No study sessions found. [white]Would you like to try again?.[/][/]", "red");
                var userInput = _userConsole.ShowMenu("", ["Yes", "No"]);

                if (userInput == "Yes")
                    DisplayYearlyReport();
                else
                    return [];
            }

            // Initialize a dictionary to store report data with stack names as keys
            var reportData = new Dictionary<string, YearlyStudySessionReport>();

            foreach (var session in studySessions)
            {
                var stackId = session.StackId;
                DateTime sessionDate = session.SessionDate;
                var score = session.Score;

                var month = sessionDate.ToString("MMMM");
                var stack = _stackRepository.GetStackById(stackId);
                var stackName = stack.Name;

                if (!reportData.ContainsKey(stackName))
                {
                    reportData[stackName] = new YearlyStudySessionReport{StackName = stackName};
                }
                reportData[stackName].MonthlyScores[month] += score;
            }

            return [.. reportData.Values]; ;
        }

        public void DisplayYearlyReport()
        {
            var reportData = GenerateYearlyReport();

            if (reportData.Count == 0)
                return;

            var table = new Table() {Border = TableBorder.Double};
            table.AddColumn("[bold yellow]Stack Name[/]");

            var months = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            foreach (var month in months)
            {
                table.AddColumn(new TableColumn($"[bold green]{month}[/]"));
            }

            bool isAlternateRow = false;
            foreach (var report in reportData)
            {
                var row = new List<string> { $"[bold cyan]{report.StackName}[/]" };
                row.AddRange(months.Select(month => 
                {
                    string color = isAlternateRow ? "royalblue1" : "chartreuse3_1";
                    return $"[{color}]{report.MonthlyScores[month]}[/]";
                }));
                table.AddRow(row.ToArray());
                isAlternateRow = !isAlternateRow;
            }

            _userConsole.WritTable(table);

            _userConsole.PrintMessage("Press any key to go to Main Menu", "blue");
            _userConsole.WaitForAnyKey();
        }

        public int CheckIfYearIsValid()
        {
            int finalDate;
            var year = _userConsole.GetUserInput("[bold]Please enter a year in [chartreuse3_1](Format: yyyy)[/][/]");

            while (!int.TryParse(year, out finalDate) || year.Length != 4)
            {
                _userConsole.PrintMessage("[bold]Invalid year format. [white]Please enter a year in [chartreuse3_1](Format: yyyy)[/][/][/].", "red");
                year = _userConsole.GetUserInput("[bold]Please enter a year in [chartreuse3_1](Format: yyyy)[/][/]");
            }

            return finalDate;
        }
    }
}