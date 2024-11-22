using FlashCards.Controllers;
using FlashCards.Data;
using FlashCards.Services;
using Spectre.Console;

namespace FlashCards.Utilities;

public static class FlashcardExtensions
{
    private static readonly StackController _stackController = new StackController(new DatabaseService());
    private static readonly FlashcardController _flashcardController = new FlashcardController(new DatabaseService());
    
    internal static string GetNumberOfFlashCards()
    {
        var numberOfFlashCards = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the [green]number of flashcards to view[/]:")
                .PromptStyle("yellow")
                .ValidationErrorMessage("[red]That's not a valid number![/]")
                .Validate(input =>
                {
                    return int.TryParse(input, out var result) && result > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Please enter a positive number![/]");
                }));
        return numberOfFlashCards;
    }

    internal static string GetFlashcardInfo(string flashcardSide)
    {
        string userInput = AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]Enter text for {flashcardSide} of flashcard:[/]")
                .PromptStyle("yellow")
                .ValidationErrorMessage("[red]input cannot be empty![/]")
                .Validate(input =>
                {
                    return string.IsNullOrWhiteSpace(input)
                        ? ValidationResult.Error("[red]Please provide a valid, non-empty value.[/]")
                        : ValidationResult.Success();
                }));
        return userInput;
    }
    
    internal static int GetStack_id(string currentStack)
    {
        List<Stack> stacks = _stackController.GetAllStacks();
        return stacks.FirstOrDefault(stack => stack.Name == currentStack)?.Id ?? 0;
    }

    internal static List<FlashCardDto> GetAllFlashcardsInOrder(List<FlashCardDto> originalFlashcards)
    {
        List<FlashCardDto> flashcards = new List<FlashCardDto>();
        int id = 1;
        foreach (FlashCardDto flashcard in originalFlashcards)
        {
            flashcards.Add(flashcard with {flashcardId = id, front = flashcard.front, back = flashcard.back});
            id++;
        }
        return flashcards;
    }

    internal static (int, string, string) GetFlashcard(string currentStack)
    {
        int id = GetFlashcard_id(currentStack);
        List<FlashCardDto> flashcards = GetAllFlashcardsInOrder(_flashcardController.GetAllFlashcards(GetStack_id(currentStack)));
        string front = "";
        string back = "";
        foreach (FlashCardDto flashcard in flashcards)
        {
            if (flashcard.flashcardId == id)
            {
                front = flashcard.front;
                back = flashcard.back;
            }
        }
        return (id, front, back);
    }
    
    private static int GetFlashcard_id(string currentStack)
    {
        FlashcardService.ViewAllFlashcards(currentStack);
        List<int> flashcardIds = GetAllFlashcardIds(currentStack);
        int ChosenFlashCardId = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter [green]Flashcard ID:[/]")
                .ValidationErrorMessage("[red]That's not a valid number![/]")
                .Validate(id =>
                {
                    return flashcardIds.Contains(id)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Please enter from the table[/]");
                }));
        return ChosenFlashCardId;
    }

    private static List<int> GetAllFlashcardIds(string currentStack)
    {
        List<FlashCardDto> flashcards = GetAllFlashcardsInOrder(_flashcardController.GetAllFlashcards(GetStack_id(currentStack)));
        List<int> flashcardIds = new List<int>();
        foreach (FlashCardDto flashcard in flashcards)
        {
            flashcardIds.Add(flashcard.flashcardId);
        }
        return flashcardIds;
    }
}