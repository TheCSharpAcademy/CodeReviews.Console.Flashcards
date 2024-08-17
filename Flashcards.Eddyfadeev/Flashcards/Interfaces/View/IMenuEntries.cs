using Spectre.Console;

namespace Flashcards.Interfaces.View;

/// <summary>
/// Represents an interface for retrieving menu entries.
/// </summary>
/// <typeparam name="TMenu">The type of menu entries.</typeparam>
internal interface IMenuEntries<TMenu> where TMenu : Enum
{
    /// <summary>
    /// Retrieves the menu entries for a specified menu.
    /// </summary>
    /// <typeparam name="TMenu">The type of menu.</typeparam>
    /// <returns>A SelectionPrompt of strings representing the menu entries.</returns>
    public SelectionPrompt<string> GetMenuEntries();
}