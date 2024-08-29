using Flashcards.Eddyfadeev.Interfaces.View.Commands;

namespace Flashcards.Eddyfadeev.Interfaces.View.Factory;

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
    /// <returns>A dictionary mapping menu entries to command functions.</returns>
    Dictionary<TMenu, Func<ICommand>> InitializeEntries();
}