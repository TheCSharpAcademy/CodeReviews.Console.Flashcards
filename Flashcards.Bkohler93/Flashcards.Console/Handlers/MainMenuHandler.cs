using Flashcards.Database;

namespace Flashcards.Handlers;

public class MainMenuHandler
{
    private ManageStackHandler manageStackHandler;
    private DbContext db;

    public MainMenuHandler(DbContext dbContext)
    {
        db = dbContext;
        manageStackHandler = new(db);
    }

    public async Task<bool> HandleChoice()
    {
        string[] options = ["Exit", ManageStackHandler.MenuName, "Study", "View Study History"];
        var choice = UI.MenuSelection("[green]Flash[/][red]cards![/] [blue]Menu[/]. Select an option below:", options);

        switch (choice)
        {
            case 0:
                return false;
            case 1:
                await manageStackHandler.Handle();
                break;
            case 2:
                break;
            case 3:
                break;
        }

        return true;
    }
}
