using Flashcards.Database;

namespace Flashcards.Handlers;

public class MainMenuHandler
{
    private ManageStackHandler manageStackHandler;
    private ViewStudyHistoryHandler viewStudyHistoryHandler;
    private StudySessionHandler studySessionHandler;
    private DbContext db;

    public MainMenuHandler(DbContext dbContext)
    {
        db = dbContext;
        manageStackHandler = new(db);
        viewStudyHistoryHandler = new(db);
        studySessionHandler = new(db);
    }

    public async Task<bool> HandleChoice()
    {
        string[] options = ["Exit", ManageStackHandler.MenuName, StudySessionHandler.MenuName, ViewStudyHistoryHandler.MenuName];
        var choice = UI.MenuSelection("[green]Flash[/][red]cards![/] [blue]Menu[/]. Select an option below:", options);

        switch (choice)
        {
            case 0:
                return false;
            case 1:
                await manageStackHandler.Handle();
                break;
            case 2:
                await studySessionHandler.Handle();
                break;
            case 3:
                await viewStudyHistoryHandler.Handle();
                break;
        }

        return true;
    }
}
