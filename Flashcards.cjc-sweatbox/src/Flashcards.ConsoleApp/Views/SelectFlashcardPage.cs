using Flashcards.ConsoleApp.Models;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to select a flashcard to perform an action on.
/// </summary>
internal class SelectFlashcardPage : BasePage
{
    #region Constants

    private const string PageTitle = "Select Flashcard";

    #endregion
    #region Properties

    internal static IEnumerable<UserChoice> PageChoices
    {
        get
        {
            return
            [
                new(0, "Close page"),
            ];
        }
    }

    #endregion
    #region Methods - Internal

    internal static FlashcardDto? Show(IReadOnlyList<FlashcardDto> flashcards)
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        var option = GetOption(flashcards);
        
        return option.Id == 0 ? null : flashcards.First(x => x.Id == option.Id);
    }

    #endregion
    #region Methods - Private

    private static UserChoice GetOption(IReadOnlyList<FlashcardDto> flashcards)
    {
        // Add the list to the existing PageChoices.
        IEnumerable<UserChoice> pageChoices = [.. flashcards.Select(x => new UserChoice(x.Id, $"{x.Question} = {x.Answer}")), .. PageChoices];

        return AnsiConsole.Prompt(
                new SelectionPrompt<UserChoice>()
                .Title(PromptTitle)
                .AddChoices(pageChoices)
                .UseConverter(c => c.Name!)
                );
    }

    #endregion
}
