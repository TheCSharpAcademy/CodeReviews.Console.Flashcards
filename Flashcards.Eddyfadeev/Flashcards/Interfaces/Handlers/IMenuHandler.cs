using Spectre.Console;

namespace Flashcards.Interfaces.Handlers;

/// <summary>
/// Represents a menu handler that handles menu interactions.
/// </summary>
/// <typeparam name="TMenu">The type of the menu entries.</typeparam>
internal interface IMenuHandler<out TMenu> where TMenu : Enum
{
    /// <summary>
    /// Handles the menu for a specific menu entries type.
    /// </summary>
    /// <typeparam name="TMenu">The type of the menu entries.</typeparam>
    internal void HandleMenu();

    /// <summary>
    /// Handles the user's choosable entry from a menu.
    /// </summary>
    /// <typeparam name="TMenu">The type of the menu.</typeparam>
    /// <param name="entries">The selection prompt representing the menu entries.</param>
    /// <returns>The chosen menu entry.</returns>
    internal TMenu HandleChoosableEntry(SelectionPrompt<string> entries);
}