using Flashcards.Database;

namespace Flashcards.Handlers;

public class ManageStackHandler(DbContext dbContext) {
    private readonly DbContext db = dbContext;

    public static string MenuName = "Manage Stacks";
    private string[] MenuOptions = ["Back to main menu", "Delete Stack", "Create Stack", "Update Stack", "View Stack"];

    public void Handle()
    {
        while (true) {

            var choice = UI.MenuSelection("[green]Manage Stacks[/] [blue]Menu[/]. Select an option below:", MenuOptions);
        
            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    //HandleDeleteStack
                    break;
                case 2:
                    HandleCreateStack();
                    break;
                case 3:
                    //HandleUpdateStack
                    break;
                case 4:
                    //HandleViewStack
                    break;
            }
        }
    }
}