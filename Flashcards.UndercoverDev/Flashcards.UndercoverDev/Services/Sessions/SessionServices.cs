using Flashcards.UndercoverDev.Extensions;
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

            var selectedStackName = _userConsole.ShowMenu("Select a [blue]Stack[/] to study", stackName);

            var retrievedStack = _stackRepository.GetStackByName(selectedStackName);
            var flashcards = _flashcardRepository.GetFlashcardsByStackId(retrievedStack.Id);

            if (flashcards.Count == 0)
            {
                _userConsole.PrintMessage("There are no flashcards in this stack. Press any key to continue.", "red");
                _userConsole.WaitForAnyKey();
                return;
            }

            int score = 0;
            int index = 1;
            
            foreach (var flashcard in flashcards)
            {
                var table = CreateTable(index,"Flashcard Id", "Question", flashcard.Question);

                _userConsole.WritTable(table);

                string userAnswer;
                do
                {
                    userAnswer = _userConsole.GetUserInput("\nPlease enter your answer to the above flashcard: ");
                }
                while (string.IsNullOrEmpty(userAnswer));

                if (userAnswer.TrimAndLower() == flashcard.Answer.TrimAndLower())
                {
                    table = CreateTable(index,"Flashcard Id", "Answer", flashcard.Answer);
                    _userConsole.WritTable(table);
                    score++;
                    _userConsole.PrintMessage($"Correct! Your current score is {score}", "green");
                }
                else
                {
                    _userConsole.PrintMessage("Incorrect!", "red");
                }

                _userConsole.PrintMessage("Press any key to continue.", "blue");
                _userConsole.WaitForAnyKey();
            }
            _userConsole.PrintMessage($"[bold]Study session completed. Your final score: {score}/{flashcards.Count}[/]", "green");

            _sessionRepository.Post(retrievedStack.Id, score, flashcards.Count);
            _userConsole.PrintMessage("Press any key to continue.", "blue");
            _userConsole.WaitForAnyKey();
        }

        public void ViewSession()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.ShowMenu("Select a [blue]Stack[/] to study", stackName);

            var retrievedStack = _stackRepository.GetStackByName(selectedStackName);
            var sessions = _sessionRepository.GetSessionsByStackId(retrievedStack.Id);

            if (sessions.Count == 0)
            {
                _userConsole.PrintMessage("There are no study sessions in this stack. Press any key to continue.", "red");
                _userConsole.WaitForAnyKey();
                return;
            }

            _userConsole.PrintMessage($"{selectedStackName} [blue]Study Sessions[/]", "green");
            var table = new Table() {Border = TableBorder.Double};
            table.AddColumn("Session Date");
            table.AddColumn("Score");

            int averageScore = 0;
            foreach (var session in sessions)
            {
                averageScore += session.Score;
                table.AddRow(session.SessionDate.ToString(), $"{session.Score}/{session.TotalQuestions}");
            }
            averageScore /= sessions.Count;

            _userConsole.WritTable(table);
            _userConsole.PrintMessage($"Average study session score: {averageScore}", "lightblue");
            _userConsole.PrintMessage("Press any key to continue.", "blue");
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
            table.AddRow(index++.ToString(), flashcardString);

            return table;
        }
    }
}