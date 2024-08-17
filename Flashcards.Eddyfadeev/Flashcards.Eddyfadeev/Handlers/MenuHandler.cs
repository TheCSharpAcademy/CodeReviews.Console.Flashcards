using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.View;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;
using Flashcards.Eddyfadeev.Extensions;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.Handlers;

/// <summary>
/// Handles the menu for a specific menu entries type.
/// </summary>
/// <typeparam name="TMenu">The type of the menu entries.</typeparam>
internal class MenuHandler<TMenu> : IMenuHandler<TMenu> where TMenu : Enum
{
    private readonly IMenuCommandFactory<TMenu> _commandFactory;
    private readonly SelectionPrompt<string> _menuEntries;

    public MenuHandler(IMenuEntries<TMenu> menuEntries, IMenuCommandFactory<TMenu> commandFactory)
    {
        _menuEntries = menuEntries.GetMenuEntries();
        _commandFactory = commandFactory;
    }

    /// <summary>
    /// Handles the menu for the specific menu entries.
    /// </summary>
    /// <typeparam name="TMenu">The type of the menu entries.</typeparam>
    public void HandleMenu()
    {
        var userChoice = HandleUserChoice(_menuEntries);
        _commandFactory.Create(userChoice).Execute();
    }

    /// <summary>
    /// Handles the user's choosable entry from a menu.
    /// </summary>
    /// <typeparam name="TMenu">The type of the menu.</typeparam>
    /// <param name="entries">The selection prompt representing the menu entries.</param>
    /// <returns>The chosen menu entry.</returns>
    public TMenu HandleChoosableEntry(SelectionPrompt<string> entries) => HandleUserChoice(entries);

    private static TMenu HandleUserChoice(SelectionPrompt<string> entries) =>
        AnsiConsole.Prompt(entries).GetValueFromDisplayName<TMenu>();
}