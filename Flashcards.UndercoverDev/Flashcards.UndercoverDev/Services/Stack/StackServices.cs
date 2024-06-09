using Flashcards.UndercoverDev.Extensions;
using Flashcards.UndercoverDev.Models;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.UserInteraction;

namespace Flashcards.UndercoverDev.Services
{
    public class StackServices : IStackServices
    {
        private readonly IUserConsole _userConsole;
        private readonly IStackRepository _stackRepository;
        private readonly IFlashcardRepository _flashcardRepository;

        public StackServices(IUserConsole userConsole, IStackRepository stackRepository, IFlashcardRepository flashcardRepository)
        {
            _userConsole = userConsole;
            _stackRepository = stackRepository;
            _flashcardRepository = flashcardRepository;
        }

        public void AddStack()
        {
            var newStackName = _userConsole.GetUserInput("Please enter the name of the Stack you would like to add (or press 0 to cancel) ");

            if (newStackName == "0")
                return;

            if (CheckIfStackExists(newStackName))
            {
                _userConsole.PrintMessage("Stack already exists. Press any key to continue.", "red");
            }
            else
            {
                _stackRepository.Post(new StackDTO
                {
                    Name = newStackName.ToTitleCase()
                });
                _userConsole.PrintMessage("1 added successfully. Press any key to continue.", "green");
            }
            _userConsole.WaitForAnyKey();
        }

        public void DeleteStack()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.ShowMenu("Select a [blue]Stack[/] to delete", stackName);

            // Delete flashcards in the selected stack
            var retrievedStack = _stackRepository.GetStackByName(selectedStackName);
            var flashcards = _flashcardRepository.GetFlashcardsByStackId(retrievedStack.Id);

            foreach (var flashcard in flashcards)
            {
                _flashcardRepository.Delete(flashcard);
            }

            if (CheckIfStackExists(selectedStackName))
            {
                _stackRepository.Delete(new Stack
                {
                    Name = selectedStackName
                });

                _userConsole.PrintMessage($"{selectedStackName} and flashcards deleted successfully. Press any key to continue", "green");
            }
            else
            {
                _userConsole.PrintMessage("Stack does not exist. Press any key to continue.", "red");
            }
            _userConsole.WaitForAnyKey();
        }

        public bool CheckIfStackExists(string stackName)
        {
            var currentStacks = _stackRepository.GetStacks();
            var stackFound = false;

            if (stackName == "None")
                return true;

            foreach (var stack in currentStacks)
            {
                if (stack.Name.TrimAndLower() == stackName.TrimAndLower())
                {
                    stackFound = true;
                    break;
                }
            }

            return stackFound;
        }
    }
}