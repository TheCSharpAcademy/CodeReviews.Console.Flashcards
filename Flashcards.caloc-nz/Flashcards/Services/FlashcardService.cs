using System.Globalization;
using CsvHelper;
using Flashcards.Data;
using Flashcards.Helpers;
using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Services;

public class FlashcardService
{
    private const int MaxTextLength = 250;
    private readonly AppDbContext _dbContext;

    public FlashcardService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private bool ValidateText(string text, string fieldName)
    {
        return ValidationHelper.ValidateString(text, fieldName, MaxTextLength);
    }

    // Add a new flashcard to the specified stack
    public void AddFlashcard(int stackId, string front, string back)
    {
        if (!ValidationHelper.ValidateId(stackId, "stack") || !ValidateText(front, "Front") ||
            !ValidateText(back, "Back"))
            return;

        var stack = _dbContext.Stacks.Find(stackId);
        if (stack == null)
        {
            ErrorHelper.DisplayError($"Stack with ID {stackId} not found.", new KeyNotFoundException());
            return;
        }

        var flashcard = new Flashcard { StackId = stackId, Front = front.Trim(), Back = back.Trim() };
        _dbContext.Flashcards.Add(flashcard);
        _dbContext.SaveChanges();
        AnsiConsole.MarkupLine($"[green]Flashcard added to stack {stackId}![/]");
    }

    // Retrieve flashcards by stack ID
    public List<Flashcard> GetFlashcardsByStack(int stackId)
    {
        if (!ValidationHelper.ValidateId(stackId, "stack"))
            return new List<Flashcard>();

        return _dbContext.Flashcards.Where(f => f.StackId == stackId).ToList();
    }

    // Update an existing flashcard
    public void UpdateFlashcard(int flashcardId, string newFront, string newBack)
    {
        if (!ValidationHelper.ValidateId(flashcardId, "flashcard") || !ValidateText(newFront, "Front") ||
            !ValidateText(newBack, "Back"))
            return;

        var flashcard = _dbContext.Flashcards.Find(flashcardId);
        if (flashcard != null)
        {
            flashcard.Front = newFront.Trim();
            flashcard.Back = newBack.Trim();
            _dbContext.SaveChanges();
            AnsiConsole.MarkupLine($"[green]Flashcard updated in stack {flashcard.StackId}![/]");
        }
        else
        {
            ErrorHelper.DisplayError($"Flashcard with ID {flashcardId} not found.", new KeyNotFoundException());
        }
    }

    // Delete a flashcard by ID
    public void DeleteFlashcard(int flashcardId)
    {
        if (!ValidationHelper.ValidateId(flashcardId, "flashcard"))
            return;

        var flashcard = _dbContext.Flashcards.Find(flashcardId);
        if (flashcard != null)
        {
            _dbContext.Flashcards.Remove(flashcard);
            _dbContext.SaveChanges();
            AnsiConsole.MarkupLine($"[green]Flashcard deleted from stack {flashcard.StackId}![/]");
        }
        else
        {
            ErrorHelper.DisplayError($"Flashcard with ID {flashcardId} not found.", new KeyNotFoundException());
        }
    }

    // Search for flashcards by a keyword within a specific stack
    public List<Flashcard> SearchFlashcards(int stackId, string keyword)
    {
        if (!ValidationHelper.ValidateId(stackId, "stack") || string.IsNullOrWhiteSpace(keyword) ||
            !ValidateText(keyword, "Search keyword"))
            return new List<Flashcard>();

        return _dbContext.Flashcards
            .Where(f => f.StackId == stackId &&
                        (EF.Functions.Like(f.Front, $"%{keyword}%") || EF.Functions.Like(f.Back, $"%{keyword}%")))
            .ToList();
    }

    // Export flashcards to a CSV file
    public void ExportFlashcardsToCsv(int? stackId, string filePath)
    {
        var flashcards = stackId.HasValue
            ? _dbContext.Flashcards.Where(f => f.StackId == stackId.Value).ToList()
            : _dbContext.Flashcards.ToList();

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(flashcards);
        }

        AnsiConsole.MarkupLine($"[green]Flashcards exported successfully to {filePath}[/]");
    }

    // Import flashcards from a CSV file into a specific stack
    public void ImportFlashcardsFromCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var flashcards = csv.GetRecords<Flashcard>().ToList();

            foreach (var flashcard in flashcards)
            {
                // Validate that the StackId exists in the database
                var stack = _dbContext.Stacks.Find(flashcard.StackId);
                if (stack == null)
                {
                    AnsiConsole.MarkupLine(
                        $"[red]Error: Stack with ID {flashcard.StackId} not found. Skipping flashcard with Front: {flashcard.Front}[/]");
                    continue; // Skip this flashcard if the StackId is invalid
                }

                // Add the flashcard to the database
                _dbContext.Flashcards.Add(flashcard);
            }

            _dbContext.SaveChanges();
        }

        AnsiConsole.MarkupLine($"[green]Flashcards imported successfully from {filePath}.[/]");
    }
}