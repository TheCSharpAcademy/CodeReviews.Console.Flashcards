using System.Xml.Linq;
using Flashcards.Database;
using Models;

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

    private void HandleCreateStack()
    {
        // enter name of stack
        var stackName = UI.StringResponse("Enter the [green]name[/] of the new stack");
d
        var stack = new CreateStackDto();
        // while (true)
            // display menu to "Add Flashcard" or "Finish Creating Stack"
            // case 0
                // get front of flash card
                // get back of flash card
                // create flashcard
            // case 1
                // save stack to database
                // add stack to cache
    }
}