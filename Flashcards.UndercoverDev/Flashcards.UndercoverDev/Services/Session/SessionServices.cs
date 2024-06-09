using Flashcards.UndercoverDev.Extensions;
using Flashcards.UndercoverDev.Models;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.Repository.Session;
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
                CreateTable(index, "Question", flashcard.Question);

                string userAnswer;
                do
                {
                    userAnswer = _userConsole.GetUserInput("\nPlease enter your answer to the above flashcard: ");
                }
                while (string.IsNullOrEmpty(userAnswer));

                if (userAnswer.TrimAndLower() == flashcard.Answer.TrimAndLower())
                {
                    CreateTable(index, "Answer", flashcard.Answer);
                    _userConsole.PrintMessage($"Correct! Your current score is {score}", "green");
                    score++;
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

        // Helper functions
        public void CreateTable(int index, string columnName, string flashcardString)
        {
            var table = new Table() {Border = TableBorder.Double};
            table.AddColumn(new TableColumn("Flashcard Id").RightAligned());
            table.AddColumn(columnName);
            table.AddRow(index++.ToString(), flashcardString);
            _userConsole.WritTable(table);
        }
    }
}