using Flashcards.Database;
using Flashcards.Models;

namespace Flashcards.Handlers;

public class ManageStackHandler(DbContext dbContext)
{
    private readonly DbContext db = dbContext;
    public const string MenuName = "Manage Stacks";

    public async Task Handle()
    {
        while (true)
        {
            string[] menuOptions = ["Back to main menu", "Delete Stack", "Create Stack", "Update Stack", "View Stack"];
            var choice = UI.MenuSelection("[green]Manage Stacks[/] [blue]Menu[/]. Select an option below:", menuOptions);

            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    // HandleDeleteStack();
                    break;
                case 2:
                    await HandleCreateStack();
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

    private async Task HandleCreateStack()
    {
        var stackName = UI.StringResponse("Enter the [green]name[/] of the new stack");

        List<CreateFlashcardDto> flashcards = [];

        while (true)
        {
            UI.Clear();
            var choice = UI.MenuSelection("Create Stack Menu", ["Add flashcard", "Finish creating stack"]);

            switch (choice)
            {
                case 0:
                    var front = UI.StringResponse("Enter the [green]Front[/] of the flashcard");
                    var back = UI.StringResponse("Enter the [yellow]Back[/] of the flashcard");

                    var flashcard = new CreateFlashcardDto(front, back);

                    flashcards.Add(flashcard);
                    break;
                case 1:
                    var stack = new CreateStackDto(stackName, flashcards);

                    await db.CreateStackAsync(stack);
                    
                    return;
            }
        }
    }
}