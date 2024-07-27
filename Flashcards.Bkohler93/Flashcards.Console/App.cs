using Flashcards.Database;
using Flashcards.Handlers;

namespace Flashcards;

public class App
{
    private readonly DbContext db;
    private MainMenuHandler mainMenuHandler;

    public App(DbContext dbContext)
    {
        db = dbContext;
        mainMenuHandler = new(db);
    }

    public async Task Run()
    {
        UI.ConfirmationMessage("[yellow]Starting app[/]");

        bool continueRunning = true;
        while (continueRunning)
        {
            continueRunning = await mainMenuHandler.HandleChoice();

            if (!continueRunning)
            {
                break;
            }
        }
    }
}
