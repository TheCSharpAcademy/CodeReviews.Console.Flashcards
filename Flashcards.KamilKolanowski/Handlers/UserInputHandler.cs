using Flashcards.KamilKolanowski.Models;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Services;

internal static class UserInputHandler
{
    internal static Cards CreateFlashcard(List<(int, string)> stacks)
    {
        var stackNames = stacks.Select(x => x.Item2).ToList();
        
        var stackChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the Stack to assign from the list:")
                .AddChoices(stackNames));
        
        var stackId = stacks.First(x => x.Item2 == stackChoice).Item1;

        var flashcardTitle = AnsiConsole.Prompt(
            new TextPrompt<string>("Provide title for the flashcard: "));

        var flashcardContent = AnsiConsole.Prompt(
            new TextPrompt<string>("Provide content for the flashcard: "));

        return new Cards { 
            StackId = stackId, 
            FlashcardTitle = flashcardTitle, 
            FlashcardContent = flashcardContent
        };
    }
}