using Flashcards.ConsoleApp.Models;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to select a stack to perform an action on.
/// </summary>
internal class SelectStackPage : BasePage
{
    #region Constants

    private const string PageTitle = "Select Stack";

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

    internal static StackDto? Show(IReadOnlyList<StackDto> stacks)
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        var option = GetOption(stacks);
        
        return option.Id == 0 ? null : stacks.First(x => x.Id == option.Id);
    }

    #endregion
    #region Methods - Private

    private static UserChoice GetOption(IReadOnlyList<StackDto> stacks)
    {
        // Add the list to the existing PageChoices.
        IEnumerable<UserChoice> pageChoices = [.. stacks.Select(x => new UserChoice(x.Id, x.Name)), .. PageChoices];

        return AnsiConsole.Prompt(
                new SelectionPrompt<UserChoice>()
                .Title(PromptTitle)
                .AddChoices(pageChoices)
                .UseConverter(c => c.Name!)
                );
    }

    #endregion
}
