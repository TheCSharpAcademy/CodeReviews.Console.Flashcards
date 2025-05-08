using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using STUDY.ConsoleApp.Flashcards.Controllers;
using STUDY.ConsoleApp.Flashcards.Enums;
using STUDY.ConsoleApp.Flashcards.Models;

namespace STUDY.ConsoleApp.Flashcards.UI;

public class ManageStudySession
{
    public static void Menu()
    {
        AnsiConsole.Clear();
        var menuOptions = AnsiConsole.Prompt(
            new SelectionPrompt<StudySessionOptions>()
                .Title("Study")
                .AddChoices(Enum.GetValues<StudySessionOptions>())
        );
        
        switch (menuOptions)
        {
            case StudySessionOptions.StartStudySession:
                StackController stackController = new();
                var stackList = stackController.ListAllStacks();
                if (stackList.IsNullOrEmpty())
                {
                    AnsiConsole.MarkupLine("[red]Add at least one stack to study.[/]");
                    AnsiConsole.MarkupLine("\nPress Any key to continue...");
                    Console.ReadKey();
                    return;
                }
                
                FlashcardController flashcardController = new();
                var stackSelection =
                    AnsiConsole.Prompt(new SelectionPrompt<Stack>().Title("Choose a stack of flashcards to start a study session:").AddChoices(stackList));
                stackSelection.Flashcards = flashcardController.ListAllFlashcards(stackSelection.Id);
                if (stackSelection.Flashcards.IsNullOrEmpty())
                {
                    AnsiConsole.MarkupLine("[red]This stack has no flashcards to study.[/]");
                    break;
                }
                
                StudySession studySession = new(stackSelection.Id);
                foreach (var flashcard in stackSelection.Flashcards)
                {
                    Console.Clear();
                    Table table = new();
                    table.AddColumns(new TableColumn("Front"));
                    table.AddRow(flashcard.Front);
                    AnsiConsole.Write(table);
                    var answer = AnsiConsole.Prompt(new TextPrompt<string>("Enter your answer to this card (or 0 to exit): ")).EscapeMarkup();
                    if (answer == "0")
                    {
                        break;
                    }
                    else if (answer.Trim().Equals(flashcard.Back.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        studySession.Score++;
                        AnsiConsole.MarkupLine("[green]Correct answer![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Wrong answer![/]");
                    }
                    AnsiConsole.MarkupLine("Press Any key to continue...");
                    Console.ReadKey();
                }
                AnsiConsole.MarkupLine($"\n[cyan]Your score is {studySession.Score} out of {stackSelection.Flashcards.Count}[/]");
                
                StudySessionController studySessionController = new();
                studySessionController.AddStudySession(studySession);
                break;
            
            case StudySessionOptions.BackToMainMenu:
                Menus.MainMenu();
                break;
        }
        AnsiConsole.MarkupLine("Press Any key to continue...");
        Console.ReadKey();
    }
}