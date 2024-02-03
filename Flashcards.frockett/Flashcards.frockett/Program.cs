using DataAccess;
using Library;

namespace Flashcards.frockett;

internal class Program
{
    static void Main(string[] args)
    {
        // cache the data access interface and initialize the database
        var dataAccess = InitDatabase();

        var inputValidation = new InputValidation();

        // All controllers
        var cardController = new CardController(dataAccess, inputValidation);
        var stackController = new StackController(dataAccess, inputValidation);
        var studySessionController = new StudySessionController(dataAccess, inputValidation);

        // View components
        var displayService = new DisplayService();
        var menuHandler = new MenuHandler(displayService, stackController, cardController, studySessionController);

        menuHandler.ShowMainMenu();
    }

    private static IDataAccess InitDatabase()
    {
        var dataAccessMethods = new SqlDataAccessMethods();
        dataAccessMethods.InitDatabase();
        return dataAccessMethods;
    }
}
