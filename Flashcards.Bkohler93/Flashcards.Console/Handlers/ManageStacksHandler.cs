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
            UI.Clear();
            string[] menuOptions = ["Back to main menu", "Delete Stack", "Create Stack", "Update Stack", "View Stack"];
            var choice = UI.MenuSelection("[green]Manage Stacks[/] [blue]Menu[/]. Select an option below:", menuOptions);

            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    await HandleDeleteStack();
                    break;
                case 2:
                    await HandleCreateStack();
                    break;
                case 3:
                    await HandleUpdateStack();
                    break;
                case 4:
                    await HandleViewStack();
                    break;
            }
        }
    }

    private async Task HandleViewStack()
    {
        UI.Clear();
        var stack = await SelectStackById(action: "view");

        if (stack == null)
        {
            return;
        }
        UI.Clear();
        var flashcards = await db.GetStackFlashcards(stack.Id);

        UI.DisplayFlashcardInfos(flashcards);
        UI.ConfirmationMessage("");
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

    private async Task HandleDeleteStack()
    {
        var stackInfos = await db.GetStacksInfosAsync();

        UI.DisplayStackInfos(stackInfos);

        StackInfoDto? stack = null;
        int stackId = -1;
        while (stack == null)
        {
            stackId = UI.IntResponse("Enter the [green]id[/] of the stack you wish to delete. [grey]Or input '0' to exit[/]");

            if (stackId == 0)
            {
                return;
            }

            stack = await db.GetStackById(stackId);
        }

        await db.DeleteStackAsync(stackId);

        return;
    }

    private async Task HandleUpdateStack()
    {
        var stack = await SelectStackById(action: "update");

        if (stack == null)
        {
            return;
        }

        bool continueUpdating = true;
        while (continueUpdating)
        {
            UI.Clear();
            var choice = UI.MenuSelection($"Update '{stack.Name}' Stack Menu",
                ["Change stack name",
                "Update flashcard",
                "Add flashcard",
                "Delete flashcard",
                "Finish updating stack"]);

            switch (choice)
            {
                case 0:
                    await HandleChangeStackName(stack);
                    break;
                case 1:
                    await HandleUpdateStackFlashcard(stack);
                    break;
                case 2:
                    await HandleAddStackFlashcard(stack);
                    break;
                case 3:
                    await HandleDeleteStackFlashcard(stack);
                    break;
                case 4:
                    continueUpdating = false;
                    break;
            }

            if (!continueUpdating)
            {
                break;
            }
        }

        return;
    }

    private async Task HandleChangeStackName(StackInfoDto stack)
    {
        var stackName = UI.StringResponseWithDefault("Enter the new name for the stack", stack.Name);
        var dto = new UpdateStackDto
        {
            Name = stackName,
            Id = stack.Id,
        };

        await db.UpdateStackAsync(dto);
        stack.Name = stackName;
    }

    private async Task<StackInfoDto?> SelectStackById(string action)
    {
        var stackInfos = await db.GetStacksInfosAsync();

        UI.DisplayStackInfos(stackInfos);

        StackInfoDto? stack = null;
        int stackId = -1;
        while (stack == null)
        {
            stackId = UI.IntResponse("Enter the [green]id[/] of the stack you wish to " + action + ". [grey]Or input '0' to exit[/]");

            if (stackId == 0)
            {
                return null;
            }

            stack = await db.GetStackById(stackId);
        }
        return stack;
    }

    private async Task HandleUpdateStackFlashcard(StackInfoDto stack)
    {
        var flashcards = await db.GetStackFlashcards(stack.Id);
        UI.DisplayFlashcardInfos(flashcards);

        FlashcardInfoDto? flashcard = null;
        int flashcardId = -1;
        while (flashcard == null)
        {
            flashcardId = UI.IntResponse("Enter the [green]id[/] of the flashcard you wish to update. [grey]Or input '0' to exit[/]");

            if (flashcardId == 0)
            {
                return;
            }

            flashcard = await db.GetFlashcardFromStackByIdAsync(stack.Id, flashcardId);
        }

        var front = UI.StringResponseWithDefault("Enter new front for the flashcard", flashcard.Front);
        var back = UI.StringResponseWithDefault("Enter new back for the flashcard", flashcard.Back);

        var dto = new UpdateFlashcardDto(front, back, flashcardId);
        await db.UpdateStackFlashcardAsync(dto);
    }

    private async Task HandleAddStackFlashcard(StackInfoDto stack)
    {
        var front = UI.StringResponse("Enter front for the flashcard");
        var back = UI.StringResponse("Enter back for the flashcard");

        var newFlashcard = new CreateStackFlashcardDto(front, back, stack.Id);

        await db.CreateStackFlashcardAsync(newFlashcard);
    }

    private async Task HandleDeleteStackFlashcard(StackInfoDto stack)
    {
        var flashcards = await db.GetStackFlashcards(stack.Id);
        UI.DisplayFlashcardInfos(flashcards);

        FlashcardInfoDto? flashcard = null;
        int flashcardId = -1;
        while (flashcard == null)
        {
            flashcardId = UI.IntResponse("Enter the [green]id[/] of the flashcard you wish to delete. [grey]Or input '0' to exit[/]");

            if (flashcardId == 0)
            {
                return;
            }

            flashcard = await db.GetFlashcardFromStackByIdAsync(stack.Id, flashcardId);
        }

        await db.DeleteFlashcard(flashcardId);
    }
}