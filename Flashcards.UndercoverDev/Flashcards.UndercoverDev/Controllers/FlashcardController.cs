using Flashcards.UndercoverDev.DataConfig;
using Flashcards.UndercoverDev.Services;
using Flashcards.UndercoverDev.Services.Session;
using Flashcards.UndercoverDev.UserInteraction;

namespace Flashcards.UndercoverDev.Controllers
{
    public class FlashcardController
    {
        private readonly IUserConsole? _userConsole;
        private readonly IDatabaseManager _databaseManager;
        private readonly IStackServices _stackServices;
        private readonly IFlashcardServices _flashcardServices;
        private readonly ISessionServices _sessionServices;
        bool closeApp;

        public FlashcardController(IUserConsole? userConsole, IDatabaseManager databaseManager, IStackServices stackServices, IFlashcardServices flashcardServices, ISessionServices sessionServices)
        {
            _userConsole = userConsole;
            _databaseManager = databaseManager;
            _stackServices = stackServices;
            _flashcardServices = flashcardServices;
            _sessionServices = sessionServices;
        }

        public void RunProgram()
        {
            // Initialize Database and Tables
            _databaseManager.InitializeDatabase();
            _databaseManager.CreateStacksTables();
            _databaseManager.CreateFlashcardsTables();
            _databaseManager.CreateSessionsTables();

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
                case "Update a Stack":
                    _stackServices.UpdateStack();
                    break;
                case "Add a Flashcard":
                    _flashcardServices.AddFlashcard();
                    break;
                case "Delete a Flashcard":
                    _flashcardServices.DeleteFlashcard();
                    break;
                case "Study Session":
                    _sessionServices.StartSession();
                    break;
                case "View Study Session by Stack":
                    _sessionServices.ViewSession();
                    break;
                case "Average Score Yearly Report":
                    _sessionServices.DisplayYearlyReport();
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