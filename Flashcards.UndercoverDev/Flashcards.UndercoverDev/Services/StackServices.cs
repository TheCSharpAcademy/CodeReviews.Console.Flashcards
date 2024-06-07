using Flashcards.UndercoverDev.Models;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.UserInteraction;

namespace Flashcards.UndercoverDev.Services
{
    public class StackServices : IStackServices
    {
        private readonly IUserConsole _userConsole;
        private readonly IStackRepository _stackRepository;

        public StackServices(IUserConsole userConsole, IStackRepository stackRepository)
        {
            _userConsole = userConsole;
            _stackRepository = stackRepository;
        }

        public void AddStack()
        {
            var newStackName = _userConsole.GetUserInput("Please enter the name of the Stack you would like to add (or press 0 to cancel) ");

            if (newStackName == "0")
                return;

            if (CheckIfStackExists(newStackName))
            {
                _userConsole.PrintMessage("Stack already exists", "red");
                return;
            }
            
            _stackRepository.Post(new StackDTO
            {
                Name = newStackName
            });
            _userConsole.PrintMessage("1 added successfully", "green");

        }

        public void DeleteStack()
        {
            var stackName = _stackRepository.GetStackNames();

            var selectedStackName = _userConsole.DeleteStackMenu(stackName);

            if (CheckIfStackExists(selectedStackName))
            {
                _stackRepository.Delete(new Stack
                {
                    Name = selectedStackName
                });

                _userConsole.PrintMessage($"{selectedStackName} deleted successfully", "green");
            }
            else
            {
                _userConsole.PrintMessage("Stack does not exist", "red");
            }
        }

        public bool CheckIfStackExists(string stackName)
        {
            var currentStacks = _stackRepository.GetStacks();
            var stackFound = false;

            foreach (var stack in currentStacks)
            {
                if (stack.Name == stackName)
                {
                    stackFound = true;
                    break;
                }
            }

            return stackFound;
        }
    }
}