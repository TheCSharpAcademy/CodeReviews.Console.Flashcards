using FlashcardsProgram.Database;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Stacks;
using FlashcardsProgram.StudySession;
using Spectre.Console;

namespace FlashcardsProgram;

public class StudySessionsController(
    StudySessionsRepository sessionsRepository,
    FlashcardsRepository cardsRepository,
    StacksController stacksController
)
{
    public StudySessionsRepository sessionsRepo = sessionsRepository;
    public FlashcardsRepository cardsRepo = cardsRepository;

    // public bool ManageStacksCards()
    // {
    //     bool showManageStacksMenu = true;
    //     bool didPressBack = false;

    //     do
    //     {
    //         Console.Clear();
    //         var selectedStack = SelectStackFromList();
    //         if (selectedStack == null || selectedStack.Id == -1)
    //         {
    //             didPressBack = true;
    //             break;
    //         }

    //         showManageStacksMenu = ManageStack(selectedStack);
    //     } while (showManageStacksMenu);

    //     return didPressBack;
    // }

    public void ShowSessionsList()
    {
        List<StudySessionDAO> sessions = sessionsRepo.List();

        if (sessions == null || sessions.Count == 0)
        {
            AnsiConsole.MarkupLine(Utils.Text.Markup("No sessions", "red"));
            Utils.ConsoleUtil.PressAnyKeyToClear();
            return;
        }

        var table = new Table();

        table.AddColumns(["#", "Date", "Score"]);

        for (int i = 0; i < sessions.Count; i++)
        {
            table.AddRow([$"{i + 1}", sessions[i].DateTime.ToString("g"), sessions[i].Score.ToString()]);
        }

        AnsiConsole.Write(table);
    }

    public void Study()
    {
        var selectedStack = stacksController.SelectStackFromList();

        if (selectedStack == null || selectedStack.Id == -1)
        {
            return;
        }

        int numCardsInStack = cardsRepo.GetNumCardsInStack(selectedStack.Id);
        int numCards = AnsiConsole.Prompt(
            new TextPrompt<int>($"How many cards for this session? (Min: 1, Max: {numCardsInStack})")
                .Validate(input =>
                {
                    if (input < 1 || input > numCardsInStack)
                    {
                        return ValidationResult.Error(
                            $"Stack has {numCardsInStack} card(s)." +
                            $"Please enter value between 1 & {numCardsInStack}."
                        );
                    }

                    return ValidationResult.Success();
                }
            )
        );

        Console.WriteLine($"SELECT first {numCards} cards.");
    }

    // public void Study()
    // {
    //     bool stackWithSameNameExists = false;
    //     string stackName;
    //     do
    //     {
    //         stacksC
    //         Console.Clear();

    //         stackName = AnsiConsole.Ask<string>("\nStack name? ");
    //         var existingStack = stacksRepo.FindByName(stackName);
    //         if (existingStack != null)
    //         {
    //             AnsiConsole.MarkupLine(
    //                 Utils.Text.Markup($"\nStack with name {stackName} already exists. Please use a different name.", "red")
    //             );
    //             stackWithSameNameExists = true;
    //         }
    //     } while (stackWithSameNameExists);

    //     var upsertedStack = id.HasValue ?
    //         stacksRepo.Update(id.Value, new UpdateStackDTO(stackName)) :
    //         stacksRepo.Create(new CreateStackDTO(stackName));

    //     if (upsertedStack == null)
    //     {
    //         AnsiConsole.MarkupLine(Utils.Text.Markup($"Could not perform operation", "red"));
    //         return null;
    //     }

    //     AnsiConsole.MarkupLine(
    //         Utils.Text.Markup($"Done", "green")
    //     );

    //     return upsertedStack;
    // }

    // public void DeleteStack(int id)
    // {
    //     bool success = stacksRepo.Delete(id);

    //     if (success)
    //     {
    //         AnsiConsole.MarkupLine(
    //          success ? Utils.Text.Markup($"Done", "green") : Utils.Text.Markup($"Failed to delete", "red")
    //         );
    //     }
    // }

    // public bool ManageStack(StackDAO stack)
    // {
    //     bool showMenu = true;
    //     do
    //     {
    //         var fetchedStack = stacksRepo.GetById(stack.Id);

    //         Console.Clear();

    //         AnsiConsole.MarkupLine($"Manage Stack - {fetchedStack.Name}");
    //         string selectedChoice = AnsiConsole.Prompt(
    //            new SelectionPrompt<string>()
    //            .AddChoices([
    //                 ManageStackMenuChoice.Back,
    //                 ManageStackMenuChoice.EditStackName,
    //                 ManageStackMenuChoice.DeleteStack,
    //                 ManageStackMenuChoice.AddFlashcard,
    //                 ManageStackMenuChoice.VEDFlashcard
    //            ])
    //        );

    //         AnsiConsole.MarkupLine(selectedChoice);

    //         switch (selectedChoice)
    //         {
    //             case ManageStackMenuChoice.EditStackName:
    //                 CreateOrUpdateStack(fetchedStack.Id);
    //                 break;
    //             case ManageStackMenuChoice.DeleteStack:
    //                 DeleteStack(fetchedStack.Id);
    //                 showMenu = false;
    //                 break;
    //             case ManageStackMenuChoice.AddFlashcard:
    //                 flashcardsController.CreateOrUpdateFlashcard(fetchedStack.Id, null);
    //                 break;
    //             case ManageStackMenuChoice.VEDFlashcard:
    //                 bool showList = true;
    //                 do
    //                 {
    //                     var selectedFlashcard = flashcardsController.SelectFlashcardFromList();
    //                     if (selectedFlashcard != null && selectedFlashcard.Id != -1)
    //                     {
    //                         showList = flashcardsController.ManageFlashcard(selectedFlashcard, fetchedStack.Id);
    //                     }
    //                     else
    //                         showList = false;
    //                 }
    //                 while (showList);
    //                 break;
    //             case ManageStackMenuChoice.Back:
    //                 return true;
    //             default:
    //                 break;
    //         }
    //     } while (showMenu);

    //     return true;
    // }
}

public static class StudySessionMenuChoice
{
    public const string EditStackName = "[blue]Edit[/] Stack Name";
    public const string DeleteStack = "[blue]Delete[/] Stack";
    public const string AddFlashcard = "Add new [yellow]Flashcard[/]";
    public const string VEDFlashcard = "Manage [yellow]flashcards[/]";
    public const string Back = "[red]<- Go back[/]";
}