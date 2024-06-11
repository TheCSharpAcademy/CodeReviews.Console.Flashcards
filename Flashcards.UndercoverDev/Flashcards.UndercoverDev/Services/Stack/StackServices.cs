using Flashcards.UndercoverDev.Extensions;
using Flashcards.UndercoverDev.Models;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.Repository.StudySessions;
using Flashcards.UndercoverDev.Services.Session;
using Flashcards.UndercoverDev.UserInteraction;

namespace Flashcards.UndercoverDev.Services
{
    public class StackServices : IStackServices
    {
        private readonly IUserConsole _userConsole;
        private readonly IStackRepository _stackRepository;
        private readonly IFlashcardRepository _flashcardRepository;
        private readonly ISessionServices _sessionServices;

        public StackServices(IUserConsole userConsole, IStackRepository stackRepository, IFlashcardRepository flashcardRepository, ISessionServices sessionServices)
        {
            _userConsole = userConsole;
            _stackRepository = stackRepository;
            _flashcardRepository = flashcardRepository;
            _sessionServices = sessionServices;
        }

        public void AddStack()
        {
            var newStackName = _userConsole.GetUserInput("Please enter a [green]stack name[/] or enter 0 to return to Main Menu: ");

            if (newStackName == "0")
                return;

            if (CheckIfStackExists(newStackName))
            {
                _userConsole.PrintMessage($"Stack name [green]{newStackName}[/] already exists. Press any key to continue.", "red");
            }
            else
            {
                _stackRepository.Post(new StackDTO
                {
                    Name = newStackName.ToTitleCase()
                });
                _userConsole.PrintMessage($"Stack name [green]{newStackName}[/] added successfully. Press any key to continue.", "white");
            }
            _userConsole.WaitForAnyKey();
        }

        public void UpdateStack()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.ShowMenu("[bold]Select a [blue]Stack[/] to update[/]", stackName);

            if (selectedStackName == "Back")
                return;

            var newStackName = _userConsole.GetUserInput("Write the name of the [green]new stack[/]:");

            _stackRepository.Put(selectedStackName, newStackName);
            _userConsole.PrintMessage($"Stack name [green]{newStackName}[/] updated successfully. Press any key to continue.", "white");
            _userConsole.WaitForAnyKey();
        }

        public void DeleteStack()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.ShowMenu("[bold]Select a [blue]Stack[/] to delete[/]", stackName);

            if (selectedStackName == "Back")
                return;

            // Delete flashcards in the selected stack
            var retrievedStack = _stackRepository.GetStackByName(selectedStackName);
            var flashcards = _flashcardRepository.GetFlashcardsByStackId(retrievedStack.Id);

            foreach (var flashcard in flashcards)
            {
                _flashcardRepository.Delete(flashcard);
            }

            // Delete Sessions
            _sessionServices.DeleteSession(retrievedStack.Id);

            if (CheckIfStackExists(selectedStackName))
            {
                _stackRepository.Delete(new Stack
                {
                    Name = selectedStackName
                });

                _userConsole.PrintMessage($"{selectedStackName}, it's flashcards and study sessions deleted successfully. [white]Press any key to continue[/]", "green");
            }
            else
            {
                _userConsole.PrintMessage("[green]Stack[green] does not exist. [white]Press any key to continue.[/]", "red");
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