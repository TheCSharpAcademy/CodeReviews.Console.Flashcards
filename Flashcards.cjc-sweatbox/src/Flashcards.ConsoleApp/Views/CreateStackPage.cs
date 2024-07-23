using Flashcards.ConsoleApp.Services;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to create a stack.
/// </summary>
internal class CreateStackPage : BasePage
{
    #region Constants

    private const string PageTitle = "Create Stack";

    #endregion
    #region Methods - Internal

    internal static StackDto? Show()
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);
                
        var name = UserInputService.GetString($"Enter the [blue]name[/] for this stack, or [blue]0[/] to cancel creating: ");
                
        return name == "0" ? null : new StackDto(name);
    }

    #endregion
}
