using FlashcardsAssist.DreamFXX.Models;
using FlashcardsAssist.DreamFXX.Data;
using Spectre.Console;

namespace FlashcardsAssist.DreamFXX.Services;
public class FlashcardsService
{
    private readonly DatabaseService _dbService;
    private readonly StacksService _stacksService;

    public FlashcardsService(DatabaseService dbService, StacksService stacksService)
    {
        _dbService = dbService;
        _stacksService = stacksService;
    }

    public async Task AddFlashcardAsync()
    {
        var front = AnsiConsole.Ask<string>("[yellow]Enter the front of the flashcard:[/]");
        if (string.IsNullOrWhiteSpace(front))
        {
            AnsiConsole.MarkupLine("[red]Front of the flashcard cannot be empty.[/]");
            return;
        }

        var back = AnsiConsole.Ask<string>("[yellow]Enter the back of the flashcard:[/]");
        if (string.IsNullOrWhiteSpace(back))
        {
            AnsiConsole.MarkupLine("[red]Back of the flashcard cannot be empty.[/]");
            return;
        }

        var stack = await _stacksService.SelectStackAsync();
        if (stack == null) return;

        front = AnsiConsole.Ask<string>("[yellow]Enter the front of the flashcard:[/]");
        back = AnsiConsole.Ask<string>("[yellow]Enter the back of the flashcard:[/]");
        try
        {
            await _dbService.AddFlashcardAsync(stack.Id, front, back);
            AnsiConsole.MarkupLine($"[green]Flashcard added to stack '{stack.Name}' successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error with adding a flashcard: {ex.Message}[/]");
        }
    }

    public async Task ViewFlashcardsAsync()
    {
        var stack = await _stacksService.SelectStackAsync();
        if (stack == null) return;

        var flashcards = await _dbService.GetFlashcardsForStackAsync(stack.Name);
        if (!flashcards.Any())
        {
            AnsiConsole.MarkupLine($"[yellow]No flashcards found in stack '{stack.Name}'.[/]");
            return;
        }

        var table = new Table()
            .Title($"[yellow]Flashcards in Stack: {stack.Name}[/]")
            .AddColumn(new TableColumn("ID").Centered())
            .AddColumn(new TableColumn("Front").LeftAligned())
            .AddColumn(new TableColumn("Back").LeftAligned());

        foreach (var card in flashcards)
        {
            table.AddRow(card.Id.ToString(), card.Front, card.Back);
        }

        AnsiConsole.Write(table);
    }

    public async Task<List<Flashcard>> GetFlashcardsForStudyAsync(string stackName)
    {
        return await _dbService.GetFlashcardsForStudyAsync(stackName);
    }
}
