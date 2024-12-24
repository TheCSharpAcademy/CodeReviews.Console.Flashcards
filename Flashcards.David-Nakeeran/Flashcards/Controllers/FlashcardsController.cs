using Flashcards.Utilities;
using Spectre.Console;

namespace Flashcards.Controllers;

class FlashcardsController
{
    private readonly Validation _validation;
    private readonly Conversion _conversion;

    public FlashcardsController(Validation validation, Conversion conversion)
    {
        _validation = validation;
        _conversion = conversion;
    }

    internal string GetFlashcardFront()
    {
        var front = AnsiConsole.Ask<string>("Please enter front of the card or 0 to return to main menu");
        front = _validation.CheckInputNullOrWhitespace("Please enter front of the card or enter 0 to return to main menu", front);
        return front;
    }

    internal string? GetFlashcardBack()
    {
        var back = AnsiConsole.Ask<string>("Please enter back of the card or 0 to return to main menu");
        back = _validation.CheckInputNullOrWhitespace("Please enter back of the card or enter 0 to return to main menu", back);
        return back;
    }

    internal (string? front, string? back) GetUpdatedFlashcardInputs()
    {
        var front = AnsiConsole.Prompt(
            new TextPrompt<string>("Please enter new front of card or 0 to return to main menu (leave blank to keep unchanged)")
                .AllowEmpty()).Trim();

        var back = AnsiConsole.Prompt(
            new TextPrompt<string>("Please enter new back of card or 0 to return to main menu (leave blank to keep unchanged)")
                .AllowEmpty()).Trim();

        front = string.IsNullOrWhiteSpace(front) ? string.Empty : front;
        back = string.IsNullOrWhiteSpace(back) ? string.Empty : back;

        return (front, back);
    }

    internal int GetFlashcardStackId()
    {
        var id = AnsiConsole.Ask<string>("Please enter stack id you want the flashcard to belong to or enter 0 to return to main menu.");
        id = _validation.CheckInputNullOrWhitespace("Please enter stack id you want the flashcard to belong to.", id);
        int stackId = _conversion.ParseInt(id, "Please try again, enter a stack id.");
        return stackId;
    }

    internal int GetFlashcardId()
    {
        var id = AnsiConsole.Ask<string>("Please enter id of the flashcard you wish to update for enter 0 to return to main menu.");
        id = _validation.CheckInputNullOrWhitespace("Please enter id of the flashcard you wish to update for enter 0 to return to main menu.", id);
        int stackId = _conversion.ParseInt(id, "Please try again.");
        return stackId;
    }

    internal string? GetFlashcardAnswer()
    {
        string? answer = Console.ReadLine();
        answer = _validation.CheckInputNullOrWhitespace("Please enter id of the flashcard you wish to update for enter 0 to return to main menu.", answer).Trim().ToLower();
        return answer;
    }

}