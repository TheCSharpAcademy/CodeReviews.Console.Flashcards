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
        private readonly IFlashcardServices _flashcardServices;
        bool closeApp;

        public FlashcardController(IUserConsole? userConsole, IDatabaseManager databaseManager, IStackServices stackServices, IFlashcardServices flashcardServices)
        {
            _userConsole = userConsole;
            _databaseManager = databaseManager;
            _stackServices = stackServices;
            _flashcardServices = flashcardServices;
        }

        public void RunProgram()
        {
            // Initialize Database and Tables
            _databaseManager.InitializeDatabase();
            _databaseManager.CreateFlashcardsTables();
            _databaseManager.CreateStacksTables();

            while (!closeApp)
            {
                Console.Clear();
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
                    _stackServices.DeleteStack();
                    break;
                case "Add a Flashcard":
                    _flashcardServices.AddFlashcard();
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