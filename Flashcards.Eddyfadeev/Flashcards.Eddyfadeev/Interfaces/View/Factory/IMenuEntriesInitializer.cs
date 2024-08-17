using Flashcards.Interfaces.View.Commands;

namespace Flashcards.Interfaces.View.Factory;

/// <summary>
/// Interface for initializing menu entries.
/// </summary>
/// <typeparam name="TMenu">The type of menu entries.</typeparam>
internal interface IMenuEntriesInitializer<TMenu> where TMenu : Enum
{
    /// <summary>
    /// Initializes the entries for a specific menu.
    /// </summary>
    /// <typeparam name="TMenu">The type of menu entries.</typeparam>
    /// <param name="menuCommandFactory">The menu command factory.</param>
    /// <returns>A dictionary mapping menu entries to command functions.</returns>
    Dictionary<TMenu, Func<ICommand>> InitializeEntries(IMenuCommandFactory<TMenu> menuCommandFactory);
}