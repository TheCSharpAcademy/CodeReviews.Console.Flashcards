using Flashcards.UndercoverDev.Extensions;
using Flashcards.UndercoverDev.Models;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.UserInteraction;

namespace Flashcards.UndercoverDev.Services
{
    public class FlashcardServices : IFlashcardServices
    {
        private readonly IUserConsole _userConsole;
        private readonly IFlashcardRepository _flashcardRepository;
        private readonly IStackRepository _stackRepository;
        private readonly IStackServices _stackServices;

        public FlashcardServices(IUserConsole userConsole, IFlashcardRepository flashcardRepository, IStackRepository stackRepository, IStackServices stackServices)
        {
            _userConsole = userConsole;
            _flashcardRepository = flashcardRepository;
            _stackServices = stackServices;
            _stackRepository = stackRepository;
        }

        public void AddFlashcard()
        {
            var IsForNewStack = IsFlashcardForNewStack();
            if (IsForNewStack)
            {
                AddFlashcardToNewStack();
            }
            else
            {
                AddFlashcardToOldStack();
            }
        }

        public void DeleteFlashcard()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.ShowMenu("[bold]Select a [blue]Stack[/] where your flashcard resides[/].\n", stackName);

            if (selectedStackName == "Back")
                return;

            // Get the flashcards associated with the selected stack
            var retrievedStack = _stackRepository.GetStackByName(selectedStackName);

            var flashcards = _flashcardRepository.GetFlashcardsByStackId(retrievedStack.Id);
            var questions = flashcards.Select(f => f.Question).ToList();

            if (flashcards.Count == 0)
            {
                _userConsole.PrintMessage("[bold]There are no flashcards in this stack.[/] [white]Press any key to continue.[/]", "red");
                _userConsole.WaitForAnyKey();
                return;
            }

            var selectedFlashcard = _userConsole.ShowMenu("[bold]Select a [blue]Flashcard[/] to delete[/]", questions);
            
            var flashcardToBeDeleted = flashcards.FirstOrDefault(f => f.Question == selectedFlashcard);

            _flashcardRepository.Delete(new Flashcard
            {
                Id = flashcardToBeDeleted.Id,
            });

            _userConsole.PrintMessage($"[bold][green]{selectedFlashcard}[/] deleted successfully. Press any key to continue[/]", "");
            
            _userConsole.WaitForAnyKey();
        }

        // Helper Methods
        public void AddFlashcardToOldStack()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.ShowMenu("[bold]Select a [blue]Stack[/] to add the flashcard to[/].\n",stackName);

            if (selectedStackName == "Back")
                return;

            if (_stackServices.CheckIfStackExists(selectedStackName))
            {
                var retrievedStack = _stackRepository.GetStackByName(selectedStackName);

                var question = ValidateQuestion().ToTitleCase();
                if (question == "0")
                    return;
                var answer = ValidateAnswer().ToTitleCase();
                if (answer == "0")
                    return;

                var newFlashcard = new FlashcardDTO
                {
                    StackId = retrievedStack.Id,
                    Question = question,
                    Answer = answer
                };

                _flashcardRepository.Post(newFlashcard);

                _userConsole.PrintMessage("1 Flashcard added successfully. Press any key to continue.", "green");
                _userConsole.WaitForAnyKey();
            }
            else
            {
                _userConsole.PrintMessage("No Stack with that name exists.", "red");
            }
        }

        public void AddFlashcardToNewStack()
        {
            var newStackName = _userConsole.GetUserInput("Please enter the name of the Stack you would like to add the Flashcard to (or press 0 to cancel) ");

            if (newStackName == "0")
                return;

            if (_stackServices.CheckIfStackExists(newStackName))
            {
                _userConsole.PrintMessage("Stack with that name already exists.", "red");
                return;
            }

            _stackRepository.Post(new StackDTO
            {
                Name = newStackName
            });
            _userConsole.PrintMessage($"[green]{newStackName}[/] added successfully.", "");
            
            var retrievedStack = _stackRepository.GetStackByName(newStackName);

            var question = ValidateQuestion().ToTitleCase();
            if (question == "0")
                return;
            var answer = ValidateAnswer().ToTitleCase();
            if (answer == "0")
                return;

            var newFlashcard = new FlashcardDTO
            {
                StackId = retrievedStack.Id,
                Question = question,
                Answer = answer,
            };

            _flashcardRepository.Post(newFlashcard);

            _userConsole.PrintMessage("[green]Flashcard[/] added successfully. Press any key to continue.", "");
            _userConsole.WaitForAnyKey();
        }

        public bool IsFlashcardForNewStack()
        {
            string userInput;
            do
            {
                userInput = _userConsole.GetUserInput("Is the Flashcard for a new stack? Y/N").ToLower();
            }
            while (userInput != "y" && userInput != "n");

            return userInput == "y";
        }

        public bool IfQuestionExists(string question)
        {
            var currentFlashcards = _flashcardRepository.GetFlashcards();
            var questionFound = false;

            foreach (var flashcard in currentFlashcards)
            {
                if (flashcard.Question.TrimAndLower() == question.TrimAndLower())
                {
                    _userConsole.PrintMessage("[bold]Question already exists.[/]", "red");
                    questionFound = true;
                    break;
                }
            }

            return questionFound;
        }

        public string ValidateQuestion()
        {
            string question;
            do
            {
                question = _userConsole.GetUserInput("Please enter the [green]question[/] for the Flashcard or press 0 to return to Main Menu: ");
            }
            while (question == "" || IfQuestionExists(question));

            return question;
        }

        public string ValidateAnswer()
        {
            string answer;
            do
            {
                answer = _userConsole.GetUserInput("Please enter the [green]answer[/] for the Flashcard or press 0 to return to Main Menu: ");
            }
            while (answer == "");

            return answer;
        }
    }
}