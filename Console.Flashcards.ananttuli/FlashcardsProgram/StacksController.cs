using FlashcardsProgram.Stacks;
using Spectre.Console;

namespace FlashcardsProgram;

public class StacksController(StacksRepository stacksRepository, FlashcardsController cardsController)
{
    public StacksRepository stacksRepo = stacksRepository;
    public FlashcardsController flashcardsController = cardsController;

    public bool ManageStacksCards()
    {
        bool showManageStacksMenu = true;
        bool didPressBack = false;

        do
        {
            Console.Clear();
            var selectedStack = SelectStackFromList();
            if (selectedStack == null || selectedStack.Id == -1)
            {
                didPressBack = true;
                break;
            }

            showManageStacksMenu = ManageStack(selectedStack);
        } while (showManageStacksMenu);

        return didPressBack;
    }

    public StackDAO? SelectStackFromList()
    {
        var stacks = stacksRepo.List();

        if (stacks == null || stacks.Count == 0)
        {
            AnsiConsole.MarkupLine(Utils.Text.Markup("No stacks available", "red"));
            Utils.ConsoleUtil.PressAnyKeyToClear();
            return null;
        }

        StackDAO selectedStack = AnsiConsole.Prompt(
            new SelectionPrompt<StackDAO>()
            .Title("Stacks")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more stacks)[/]")
            .AddChoices([new StackDAO(-1, Utils.Text.Markup("<- Go back", "red")), .. stacks])
            .EnableSearch()
        );

        return selectedStack;
    }

    public StackDAO? CreateOrUpdateStack(int? id = null)
    {
        bool stackWithSameNameExists = false;
        string stackName;
        do
        {
            Console.Clear();

            stackName = AnsiConsole.Ask<string>("\nStack name? ");
            var existingStack = stacksRepo.FindByName(stackName);
            if (existingStack != null)
            {
                AnsiConsole.MarkupLine(
                    Utils.Text.Markup($"\nStack with name {stackName} already exists. Please use a different name.", "red")
                );
                stackWithSameNameExists = true;
            }
        } while (stackWithSameNameExists);

        var upsertedStack = id.HasValue ?
            stacksRepo.Update(id.Value, new UpdateStackDTO(stackName)) :
            stacksRepo.Create(new CreateStackDTO(stackName));

        if (upsertedStack == null)
        {
            AnsiConsole.MarkupLine(Utils.Text.Markup($"Could not perform operation", "red"));
            return null;
        }

        AnsiConsole.MarkupLine(
            Utils.Text.Markup($"Done", "green")
        );

        return upsertedStack;
    }

    public void DeleteStack(int id)
    {
        bool success = stacksRepo.Delete(id);

        if (success)
        {
            AnsiConsole.MarkupLine(
             success ? Utils.Text.Markup($"Done", "green") : Utils.Text.Markup($"Failed to delete", "red")
            );
        }
    }

    public bool ManageStack(StackDAO stack)
    {
        bool showMenu = true;
        do
        {
            var fetchedStack = stacksRepo.GetById(stack.Id);

            Console.Clear();

            AnsiConsole.MarkupLine($"Manage Stack - {fetchedStack.Name}");
            string selectedChoice = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
               .AddChoices([
                    ManageStackMenuChoice.Back,
                    ManageStackMenuChoice.EditStackName,
                    ManageStackMenuChoice.DeleteStack,
                    ManageStackMenuChoice.AddFlashcard,
                    ManageStackMenuChoice.VEDFlashcard
               ])
           );

            AnsiConsole.MarkupLine(selectedChoice);

            switch (selectedChoice)
            {
                case ManageStackMenuChoice.EditStackName:
                    CreateOrUpdateStack(fetchedStack.Id);
                    break;
                case ManageStackMenuChoice.DeleteStack:
                    DeleteStack(fetchedStack.Id);
                    showMenu = false;
                    break;
                case ManageStackMenuChoice.AddFlashcard:
                    flashcardsController.CreateOrUpdateFlashcard(fetchedStack.Id, null);
                    break;
                case ManageStackMenuChoice.VEDFlashcard:
                    bool showList = true;
                    do
                    {
                        var selectedFlashcard = flashcardsController.SelectFlashcardFromList();
                        if (selectedFlashcard != null && selectedFlashcard.Id != -1)
                        {
                            showList = flashcardsController.ManageFlashcard(selectedFlashcard, fetchedStack.Id);
                        }
                        else
                            showList = false;
                    }
                    while (showList);
                    break;
                case ManageStackMenuChoice.Back:
                    return true;
                default:
                    break;
            }
        } while (showMenu);

        return true;
    }
}

public static class ManageStackMenuChoice
{
    public const string EditStackName = "[blue]Edit[/] Stack Name";
    public const string DeleteStack = "[blue]Delete[/] Stack";
    public const string AddFlashcard = "Add new [yellow]Flashcard[/]";
    public const string VEDFlashcard = "Manage [yellow]flashcards[/]";
    public const string Back = "[red]<- Go back[/]";
}