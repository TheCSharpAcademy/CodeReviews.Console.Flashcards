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
            else
            {
                _stackRepository.Post(new StackDTO
                {
                    Name = newStackName
                });
            }

        }

        public bool CheckIfStackExists(string stackName)
        {
            // Check if Stack Name already exists
            return false;
        }
    }
}