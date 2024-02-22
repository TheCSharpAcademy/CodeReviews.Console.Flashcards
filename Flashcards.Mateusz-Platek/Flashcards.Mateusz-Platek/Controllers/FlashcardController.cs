using Flashcards.Mateusz_Platek.Models;
using Flashcards.Mateusz_Platek.Repositories;
using Spectre.Console;

namespace Flashcards.Mateusz_Platek.Controllers;

public static class FlashcardController
{
    private static StacksRepository stacksRepository = new StacksRepository();
    private static FlashcardsRepository flashcardsRepository = new FlashcardsRepository();
    
    private static Flashcard? SelectFlashcard(string stackName)
    {
        List<Flashcard> flashcards = flashcardsRepository.GetFlashcardsOfStack(stackName);

        if (flashcards.Count == 0)
        {
            return null;
        }
        
        return AnsiConsole.Prompt(
            new SelectionPrompt<Flashcard>()
                .Title("[bold red]Select flashcard:[/]")
                .AddChoices(flashcards)
                .UseConverter(flashcard=> $"{flashcards.IndexOf(flashcard) + 1} - {flashcard.word} - {flashcard.translation}")
        );
    }
    
    public static void DisplayFlashcards(string stackName)
    {
        Table table = new Table()
            .Title("[bold red]Flashcards[/]")
            .AddColumn("[bold yellow]Id[/]")
            .AddColumn("[bold darkorange]Word[/]")
            .AddColumn("[bold purple]Translation[/]");

        List<FlashcardDTO> flashcardsOfStack = flashcardsRepository.GetFlashcardsDTOOfStack(stackName);
        
        foreach (FlashcardDTO flashcard in flashcardsOfStack)
        {
            table.AddRow(
                $"[bold yellow]{flashcardsOfStack.IndexOf(flashcard) + 1}[/]", 
                $"[bold darkorange]{flashcard.word}[/]", 
                $"[bold purple]{flashcard.translation}[/]"
                );
        }
        
        AnsiConsole.Write(table);
    }
    
    public static void AddFlashcard(string stackName)
    {
        Stack? stack = stacksRepository.GetStack(stackName);
        if (stack == null)
        {
            return;
        }

        string word = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold green]Insert word:[/]")
        );
        
        string translation = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold darkorange]Insert translation:[/]")
        );
        
        flashcardsRepository.AddFlashcard(stack.stackId, word, translation);
        
        AnsiConsole.Write(
            new Markup("[bold green]Flashcard added[/]\n")
        );
    }
    
    public static void UpdateFlashcard(string stackName)
    {
        Flashcard? flashcard = SelectFlashcard(stackName);
        if (flashcard == null)
        {
            AnsiConsole.Write(new Markup("[bold red]Stack is empty[/]\n"));
            return;
        }

        Stack? stack = stacksRepository.GetStack(stackName);
        if (stack == null)
        {
            return;
        }
        
        string newWord = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold green]Insert new word:[/]")
        );
        
        string newTranslation = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold darkorange]Insert new translation:[/]")
        );
        
        flashcardsRepository.UpdateFlashcard(flashcard.flashcardId, stack.stackId, newWord, newTranslation);
        
        AnsiConsole.Write(
            new Markup("[bold green]Flashcard updated[/]\n")
        );
    }
    
    public static void DeleteFlashcard(string stackName)
    {
        Flashcard? flashcard = SelectFlashcard(stackName);
        if (flashcard == null)
        {
            AnsiConsole.Write(new Markup("[bold red]Stack is empty[/]\n"));
            return;
        }
        
        flashcardsRepository.DeleteFlashcard(flashcard.flashcardId);
        
        AnsiConsole.Write(
            new Markup("[bold green]Flashcard removed[/]\n")
        );
    }
}