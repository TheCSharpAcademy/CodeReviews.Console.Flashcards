using Flashcards.KamilKolanowski.Dtos.StudySessions;
using Flashcards.KamilKolanowski.Models;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Handlers;

internal class UserInputHandler
{
    internal CreateCardDto CreateFlashcard(List<(int, string)> stacks)
    {
        var stackNames = stacks.Select(s => s.Item2).ToList();

        if (stackNames.Any())
        {
            var stackChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose the Stack to assign from the list:")
                    .AddChoices(stackNames)
            );

            var stackId = stacks.First(x => x.Item2 == stackChoice).Item1;

            var flashcardTitle = AnsiConsole.Prompt(
                new TextPrompt<string>("Provide title for the flashcard: ")
            );

            var flashcardContent = AnsiConsole.Prompt(
                new TextPrompt<string>("Provide content for the flashcard: ")
            );

            return new CreateCardDto()
            {
                StackId = stackId,
                FlashcardTitle = flashcardTitle,
                FlashcardContent = flashcardContent,
            };
        }

        AnsiConsole.MarkupLine(
            @"There's no Stack created.
                          Please create Stack first."
        );
        return null;
    }

    internal CreateStackDto CreateStack()
    {
        var newStackName = AnsiConsole.Prompt(
            new TextPrompt<string>("Provide title for the new stack: ")
        );

        var newDescription = AnsiConsole.Prompt(
            new TextPrompt<string>("Provide description for the new stack: ")
        );

        return new CreateStackDto() { StackName = newStackName, Description = newDescription };
    }
    
}
