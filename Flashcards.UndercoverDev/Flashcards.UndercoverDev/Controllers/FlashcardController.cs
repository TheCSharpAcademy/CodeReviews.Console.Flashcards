using Flashcards.UndercoverDev.DataConfig;
using Flashcards.UndercoverDev.Services;
using Flashcards.UndercoverDev.UserInteraction;

namespace Flashcards.UndercoverDev.Controllers
{
    public class FlashcardController
    {
        private readonly IUserConsole? _userConsole;
        private readonly IDatabaseManager _databaseManager;
        private readonly IStackServices _stackServices;
        bool closeApp;

        public FlashcardController(IUserConsole? userConsole, IDatabaseManager databaseManager, IStackServices stackServices)
        {
            _userConsole = userConsole;
            _databaseManager = databaseManager;
            _stackServices = stackServices;
        }

        public void RunProgram()
        {
            // Initialize Database and Tables
            _databaseManager.InitializeDatabase();
            _databaseManager.CreateFlashcardsTables();
            _databaseManager.CreateStacksTables();

            while (!closeApp)
            {
                // Display Main Menu
                var userInput = _userConsole.MainMenu();
                // Process User Input
                ProcessUserInput(userInput);
            }

        }

        public void ProcessUserInput(string input)
        {
            switch (input)
            {
                case "Add a Stack":
                    _stackServices.AddStack();
                    break;
                case "Delete a Stack":
                    break;
                case "Add a Flashcard":
                    break;
                case "Delete a Flashcard":
                    break;
                case "Study Session":
                    break;
                case "View Study Session by Stack":
                    break;
                case "Average Score Yearly Report":
                    break;
                case "Exit":
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}