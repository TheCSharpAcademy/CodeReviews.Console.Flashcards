using Flashcards.UndercoverDev.UserInteraction;

namespace Flashcards.UndercoverDev.Services
{
    public class StackServices : IStackServices
    {
        private readonly IUserConsole _userConsole;

        public StackServices(IUserConsole userConsole)
        {
            _userConsole = userConsole;
        }

        public void AddStack()
        {
            // Get Input for Stack Name
            // Check if Name already exists
            // Check if input is 0
            // Add Stack

            var name = _userConsole.GetUserInput("Please enter the name of the Stack you would like to add (or press 0 to cancel) ");

            if (name == "0")
                return;

            if (CheckIfStackExists(name))
            {
                _userConsole.PrintMessage("Stack already exists", "red");
                return;
            }
            else
            {
                // Post Stacks
            }

        }

        public bool CheckIfStackExists(string stackName)
        {
            // Check if Stack Name already exists
            return false;
        }
    }
}