using Flashcards.Config;
using Flashcards.Data;
using Flashcards.Helpers;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.UI;

public class FlashcardMenu
{
    public static void Show(DatabaseConfig config)
    {
        using var dbContext = new AppDbContext(config);
        var flashcardService = new FlashcardService(dbContext);
        var isRunning = true;

        while (isRunning)
            try
            {
                var flashcardAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose a flashcard operation")
                        .AddChoices("Add Flashcard", "View Flashcards", "Search Flashcards", "Update Flashcard",
                            "Delete Flashcard", "Back"));

                switch (flashcardAction)
                {
                    case "Add Flashcard":
                        var stackId = AnsiConsole.Ask<int>("Enter the stack ID to add flashcard:");
                        if (!ValidationHelper.ValidateId(stackId, "stack")) break;

                        var front = AnsiConsole.Ask<string>("Enter front text:");
                        var back = AnsiConsole.Ask<string>("Enter back text:");
                        if (!ValidationHelper.ValidateString(front, "Front text", 250) ||
                            !ValidationHelper.ValidateString(back, "Back text", 250)) break;

                        flashcardService.AddFlashcard(stackId, front, back);
                        break;

                    case "View Flashcards":
                        var viewStackId = AnsiConsole.Ask<int>("Enter the stack ID to view flashcards:");
                        if (!ValidationHelper.ValidateId(viewStackId, "stack")) break;

                        var flashcards = flashcardService.GetFlashcardsByStack(viewStackId);
                        if (flashcards.Count == 0)
                            AnsiConsole.MarkupLine("[yellow]No flashcards found for this stack.[/]");
                        else
                            foreach (var flashcard in flashcards)
                                AnsiConsole.MarkupLine(
                                    $"[yellow]Id:[/] {flashcard.Id} [yellow]Front:[/] {flashcard.Front} [yellow]Back:[/] {flashcard.Back}");
                        break;

                    case "Search Flashcards":
                        var searchStackId = AnsiConsole.Ask<int>("Enter the stack ID to search in:");
                        if (!ValidationHelper.ValidateId(searchStackId, "stack")) break;

                        var keyword = AnsiConsole.Ask<string>("Enter the keyword to search for:");
                        if (!ValidationHelper.ValidateString(keyword, "Search keyword", 250)) break;

                        var searchResults = flashcardService.SearchFlashcards(searchStackId, keyword);
                        if (searchResults.Count == 0)
                            AnsiConsole.MarkupLine(
                                $"[yellow]No flashcards found with the keyword '{keyword}' in stack {searchStackId}.[/]");
                        else
                            foreach (var flashcard in searchResults)
                                AnsiConsole.MarkupLine(
                                    $"[yellow]Id:[/] {flashcard.Id} [yellow]Front:[/] {flashcard.Front} [yellow]Back:[/] {flashcard.Back}");
                        break;

                    case "Update Flashcard":
                        var flashcardIdToUpdate = AnsiConsole.Ask<int>("Enter flashcard ID to update:");
                        if (!ValidationHelper.ValidateId(flashcardIdToUpdate, "flashcard")) break;

                        var newFront = AnsiConsole.Ask<string>("Enter new front text:");
                        var newBack = AnsiConsole.Ask<string>("Enter new back text:");
                        if (!ValidationHelper.ValidateString(newFront, "Front text", 250) ||
                            !ValidationHelper.ValidateString(newBack, "Back text", 250)) break;

                        flashcardService.UpdateFlashcard(flashcardIdToUpdate, newFront, newBack);
                        break;

                    case "Delete Flashcard":
                        var flashcardIdToDelete = AnsiConsole.Ask<int>("Enter flashcard ID to delete:");
                        if (!ValidationHelper.ValidateId(flashcardIdToDelete, "flashcard")) break;

                        flashcardService.DeleteFlashcard(flashcardIdToDelete);
                        break;

                    case "Back":
                        isRunning = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
            }
    }
}