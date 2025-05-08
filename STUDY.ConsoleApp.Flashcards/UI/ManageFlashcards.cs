using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using STUDY.ConsoleApp.Flashcards.Controllers;
using STUDY.ConsoleApp.Flashcards.Enums;
using STUDY.ConsoleApp.Flashcards.Models;
using STUDY.ConsoleApp.Flashcards.Models.DTOs;

namespace STUDY.ConsoleApp.Flashcards.UI;

public static class ManageFlashcards
{
    public static void Menu()
    {
        AnsiConsole.Clear();
        StackController stackController = new();
        var stackList = stackController.ListAllStacks();
        if (stackList.IsNullOrEmpty())
        {
            AnsiConsole.MarkupLine("[red]Add at least one stack to interact with its flashcards.[/]");
            AnsiConsole.MarkupLine("\nPress Any key to continue...");
            Console.ReadKey();
            return;
        }
        var stackSelection =
            AnsiConsole.Prompt(new SelectionPrompt<Stack>().Title("Choose a stack of flashcards to interact with:").AddChoices(stackList));
        FlashcardController flashcardController = new();
        while (true)
        {
            Console.Clear();
            var flashcardsList = flashcardController.ListAllFlashcards(stackSelection.Id);
            var menuOptions = AnsiConsole.Prompt(
                new SelectionPrompt<ManageFlashcardsOptions>()
                    .Title($"Manage Flashcards from [green]{stackSelection.Name}[/] stack")
                    .AddChoices(Enum.GetValues<ManageFlashcardsOptions>())
            );

            switch (menuOptions)
            {
                case ManageFlashcardsOptions.ChangeCurrentStack:
                    
                    stackSelection = AnsiConsole.Prompt(new SelectionPrompt<Stack>().Title("Choose a stack of flashcards to interact with:").AddChoices(stackList));
                    break;
                
                case ManageFlashcardsOptions.CreateFlashcard:
                    AnsiConsole.MarkupLine("Creating a new flashcard...");
                    var frontFlashcard = AnsiConsole.Prompt(new TextPrompt<string>("Enter front of the flashcard (or 0 to exit): ").Validate(s => s.Length <= 100, "[yellow]Text should be less than or equal 100 characters.[/]")).EscapeMarkup();
                    if (frontFlashcard == "0")
                        break;
                    
                    var backFlashcard = AnsiConsole.Prompt(new TextPrompt<string>("Enter back of the flashcard (or 0 to exit): ").Validate(s => s.Length <= 100, "[yellow]Text should be less than or equal 100 characters.[/]")).EscapeMarkup();
                    if (backFlashcard == "0")
                    {
                        AnsiConsole.MarkupLine("[yellow]Creating a flashcard is cancelled.[/]");
                        break;
                    }
                    
                    flashcardController.CreateFlashcard(stackSelection.Id, frontFlashcard, backFlashcard );

                    AnsiConsole.MarkupLine("\n[green]Flashcard created successfully.[/]");
                    break;
                
                case ManageFlashcardsOptions.ViewFlashcards:
                    
                    if (flashcardsList.IsNullOrEmpty())
                    {
                        AnsiConsole.MarkupLine("[red]There are no flashcards to view.[/]");
                        break;
                    }
                    
                    Table table = new();
                    table.AddColumns(new TableColumn("Id").Centered(), new TableColumn("Front").Centered(), new TableColumn("Back").Centered());

                    foreach (var flashcard in flashcardsList)
                    {
                        table.AddRow(flashcard.ViewId.ToString(), flashcard.Front, flashcard.Back);
                    }
                    AnsiConsole.Write(table);
                    break;
                
                case ManageFlashcardsOptions.EditFlashcard:
                    
                    if (flashcardsList.IsNullOrEmpty())
                    {
                        AnsiConsole.MarkupLine("[red]There are no flashcards to edit.[/]");
                        break;
                    }
                    Console.Clear();
                    AnsiConsole.MarkupLine("Editing a flashcard...");
                    var flashcardSelection = AnsiConsole.Prompt(new SelectionPrompt<FlashcardDto>().Title("Choose a flashcard to edit:").AddChoices(flashcardsList));
                    var editOption = AnsiConsole.Prompt(new SelectionPrompt<EditFlashcardOptions>().Title("Choose an option to edit a flashcard:").AddChoices(Enum.GetValues<EditFlashcardOptions>()));
                    switch (editOption)
                    { 
                        case EditFlashcardOptions.EditFront:
                            AnsiConsole.MarkupLine($"You will edit the front [yellow]'{flashcardSelection.Front}'[/]");
                            var editFrontFlashcard = AnsiConsole.Prompt(new TextPrompt<string>("Enter new front of the flashcard (or 0 to exit): ").Validate(s => s.Length <= 100, "[yellow]Text should be less than or equal 100 characters.[/]")).EscapeMarkup();
                            if (editFrontFlashcard == "0")
                                break;
                            
                            flashcardController.EditFlashcard(flashcardSelection.RealId, EditFlashcardOptions.EditFront, editFrontFlashcard);
                            AnsiConsole.MarkupLine("\n[green]Flashcard edited successfully.[/]");
                            break;
                        case EditFlashcardOptions.EditBack:
                            AnsiConsole.MarkupLine($"You will edit the back [yellow]'{flashcardSelection.Back}'[/]");
                            var editBackFlashcard = AnsiConsole.Prompt(new TextPrompt<string>("Enter new back of the flashcard (or 0 to exit): ").Validate(s => s.Length <= 100, "[yellow]Text should be less than or equal 100 characters.[/]")).EscapeMarkup();
                            if (editBackFlashcard == "0")
                                break;
                            
                            flashcardController.EditFlashcard(flashcardSelection.RealId, EditFlashcardOptions.EditBack, editBackFlashcard);
                            AnsiConsole.MarkupLine("\n[green]Flashcard edited successfully.[/]");
                            break;
                    }
                    
                    break;
                
                case ManageFlashcardsOptions.DeleteFlashcard:
                    
                    if (flashcardsList.IsNullOrEmpty())
                    {
                        AnsiConsole.MarkupLine("[red]There are no flashcards to delete.[/]");
                        break;
                    }
                    
                    Console.Clear();
                    var deleteSelection = AnsiConsole.Prompt(new SelectionPrompt<FlashcardDto>().Title("Choose a flashcard to delete:").AddChoices(flashcardsList));
                    var confirmation = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"[red]Are you sure you want to delete '{deleteSelection}' flashcard?[/]").AddChoices("Yes", "No"));
                    if (confirmation == "Yes")
                    {
                        flashcardController.DeleteFlashcard(deleteSelection.RealId);
                        AnsiConsole.MarkupLine("[green]Flashcard deleted successfully.[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[cyan]Flashcard won't be deleted.[/]");
                    }
                    break;
                
                case ManageFlashcardsOptions.BackToMainMenu:
                    Menus.MainMenu();
                    break;
            }
            AnsiConsole.MarkupLine("Press Any key to continue...");
            Console.ReadKey();
        }
    }
}