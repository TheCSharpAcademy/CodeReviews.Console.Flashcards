using Flashcards.ConsoleApp.Models;
using Flashcards.ConsoleApp.Services;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to update a stack.
/// </summary>
internal class UpdateStackPage : BasePage
{
    #region Constants

    private const string PageTitle = "Update Stack";

    #endregion
    #region Methods

    internal static StackDto? Show(StackDto stack)
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        // Show user the stack which is being updated.
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddRow(stack.Id.ToString(), stack.Name);

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        var name = UserInputService.GetString($"Enter the updated [blue]name[/] for this stack, or [blue]0[/] to cancel updating: ");
                
        return name == "0" ? null : new StackDto(name);
    }

    #endregion
}
