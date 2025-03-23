using FlashcardsAssist.DreamFXX.Models;
using FlashcardsAssist.DreamFXX.Data;
using Spectre.Console;

namespace FlashcardsAssist.DreamFXX.Services;
public class StudyEngine
{
    private readonly DatabaseService _dbService;
    private readonly SessionsService _sessionsService;
    private readonly StacksService _stacksService;
    private readonly FlashcardsService _flashcardsService;

    public StudyEngine(
        DatabaseService dbService, 
        SessionsService sessionsService, 
        StacksService stacksService,
        FlashcardsService flashcardsService)
    {
        _dbService = dbService;
        _sessionsService = sessionsService;
        _stacksService = stacksService;
        _flashcardsService = flashcardsService;
    }

    public async Task StartStudySessionAsync()
    {
        var stack = await _stacksService.SelectStackAsync();
        if (stack == null) return;

        var flashcards = await _flashcardsService.GetFlashcardsForStudyAsync(stack.Name);
        
        if (!flashcards.Any())
        {
            AnsiConsole.MarkupLine($"[red]No flashcards found in stack '{stack.Name}'. Add some flashcards first.[/]");
            return;
        }

        AnsiConsole.MarkupLine($"[green]Starting study session for stack '{stack.Name}'[/]");
        AnsiConsole.MarkupLine("[yellow]Press any key to reveal the answer. Type 'correct' or 'incorrect' after reviewing.[/]");
        AnsiConsole.MarkupLine("[yellow]Type 'quit' to end the session.[/]");

        int totalCards = flashcards.Count;
        int correctAnswers = 0;
        var random = new Random();
        var remainingCards = new List<Flashcard>(flashcards);

        while (remainingCards.Any())
        {
            // Get a random card from the remaining cards
            int index = random.Next(remainingCards.Count);
            var card = remainingCards[index];

            // Show front of card
            AnsiConsole.MarkupLine($"[blue]Question: {card.Front}[/]");
            Console.ReadKey(true);

            // Show back of card
            AnsiConsole.MarkupLine($"[green]Answer: {card.Back}[/]");

            // Ask if user got it right
            var response = AnsiConsole.Ask<string>("[yellow]Correct/Incorrect/Quit (c/i/q):[/]").ToLower();
            
            if (response == "q" || response == "quit")
            {
                break;
            }
            else if (response == "c" || response == "correct")
            {
                correctAnswers++;
                remainingCards.RemoveAt(index);
            }
            else if (response == "i" || response == "incorrect")
            {
                // Put the card back in, but move it later in the deck
                remainingCards.RemoveAt(index);
                remainingCards.Add(card);
            }
            
            Console.WriteLine();
        }

        int score = (int)Math.Round((double)correctAnswers / totalCards * 100);
        AnsiConsole.MarkupLine($"[green]Session completed! Your score: {score}% ({correctAnswers}/{totalCards})[/]");

        await _sessionsService.RecordStudySessionAsync(stack.Id, score);
        AnsiConsole.MarkupLine("[green]Study session recorded![/]");
    }
} 