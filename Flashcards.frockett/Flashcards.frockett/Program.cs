using DataAccess;
using Library;

namespace Flashcards.frockett;

internal class Program
{
    static void Main(string[] args)
    {
        // cache the data access interface and initialize the database
        var dataAccess = InitDatabase();

        // All controllers
        var cardController = new CardController(dataAccess);
        var stackController = new StackController(dataAccess);
        var studySessionController = new StudySessionController(dataAccess);

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
