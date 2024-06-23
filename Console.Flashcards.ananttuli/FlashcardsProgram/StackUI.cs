using FlashcardsProgram.Stacks;
using Spectre.Console;

namespace FlashcardsProgram;

public class StackUI(StacksRepository stacksRepository, FlashcardUI flashUI)
{
    public StacksRepository stacksRepo = stacksRepository;
    public FlashcardUI flashcardUI = flashUI;

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
            .Title("S T A C K S")
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

        AnsiConsole.MarkupLine($"Manage Stack - {stack.Name}");
        do
        {
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
                    CreateOrUpdateStack(stack.Id);
                    break;
                case ManageStackMenuChoice.DeleteStack:
                    DeleteStack(stack.Id);
                    showMenu = false;
                    break;
                case ManageStackMenuChoice.AddFlashcard:
                    flashcardUI.CreateOrUpdateFlashcard(stack.Id, null);
                    break;
                case ManageStackMenuChoice.VEDFlashcard:
                    bool showList = true;
                    do
                    {
                        var selectedFlashcard = flashcardUI.SelectFlashcardFromList();
                        if (selectedFlashcard != null && selectedFlashcard.Id != -1)
                        {
                            showList = flashcardUI.ManageFlashcard(selectedFlashcard, stack.Id);
                        }
                        else
                            showList = false;
                    }
                    while (showList);
                    break;
                case ManageStackMenuChoice.Back:
                    return false;
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
    public const string AddFlashcard = "[blue]Add Flashcard[/]";
    public const string VEDFlashcard = "[blue]View/Edit/Delete flashcards[/]";
    public const string Back = "[red]<- Go back[/]";
}