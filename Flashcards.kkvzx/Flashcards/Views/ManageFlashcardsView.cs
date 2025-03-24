using Flashcards.Models.Dtos;
using Flashcards.shared;
using Flashcards.Views.utils;
using Spectre.Console;

namespace Flashcards.Views;

public class ManageFlashcardsView
{
    public ManageFlashcardsOption ShowMenu(List<FlashcardDto> flashcards, string currentStack)
    {
        AnsiConsole.Clear();
        DtosDisplay.DisplayFlashcards(flashcards, currentStack);
        return DisplayOptions(flashcards.Count > 0);
    }

    public FlashcardDto GetNewFlashcard(int stackId)
    {
        var input = new UserInput();

        AnsiConsole.Clear();
        var frontText = input.GetText("Enter flashcard front text: ");
        var backText = input.GetText("Enter flashcard back text: ");

        return new FlashcardDto(stackId, frontText, backText);
    }

    public FlashcardDto GetNewFlashcard(FlashcardDto defaultFlashcard)
    {
        var input = new UserInput();

        AnsiConsole.Clear();
        var frontText = input.GetText($"Enter flashcard front text: ", defaultFlashcard.FrontText);
        var backText = input.GetText("Enter flashcard back text: ", defaultFlashcard.BackText);

        return new FlashcardDto(defaultFlashcard.Id, defaultFlashcard.StackId, frontText, backText);
    }

    private ManageFlashcardsOption DisplayOptions(bool hasFlashcards)
    {
        var menuDict = hasFlashcards
            ? new Dictionary<string, ManageFlashcardsOption>
            {
                { "Back to menu", ManageFlashcardsOption.BackToMenu },
                { "Change stack", ManageFlashcardsOption.ChangeStack },
                { "Add new flashcard", ManageFlashcardsOption.Create },
                { "Modify flashcard", ManageFlashcardsOption.Update },
                { "Delete flashcard", ManageFlashcardsOption.Delete },
            }
            : new Dictionary<string, ManageFlashcardsOption>
            {
                { "Back to menu", ManageFlashcardsOption.BackToMenu },
                { "Change stack", ManageFlashcardsOption.ChangeStack },
                { "Add new flashcard", ManageFlashcardsOption.Create },
            };

        AnsiConsole.Write(new Rule());

        var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .AddChoices(menuDict.Keys));

        return menuDict[selection];
    }
}