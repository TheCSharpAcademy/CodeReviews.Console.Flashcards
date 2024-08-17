using Flashcards.Interfaces.View.Commands;

namespace Flashcards.Interfaces.View.Factory;

/// <summary>
/// Represents a factory for creating menu commands.
/// </summary>
/// <typeparam name="TMenu">The type of menu entries.</typeparam>
internal interface IMenuCommandFactory<in TMenu> where TMenu : Enum
{
    /// <summary>
    /// Creates a menu command based on the provided entry from a menu command factory.
    /// </summary>
    /// <typeparam name="TMenu">The type of the menu entry</typeparam>
    /// <param name="entry">The entry from the menu</param>
    /// <returns>An ICommand instance based on the provided entry</returns>
    ICommand Create(TMenu entry);
}