using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;

namespace Flashcards.Eddyfadeev.View.Factory;

/// <summary>
/// Represents a factory for creating menu commands.
/// </summary>
/// <typeparam name="TMenu">The type of menu entries.</typeparam>
internal class MenuCommandFactory<TMenu> : IMenuCommandFactory<TMenu> where TMenu : Enum
{
    private readonly Dictionary<TMenu, Func<ICommand>> _entriesFactory;
    
    public MenuCommandFactory(IMenuEntriesInitializer<TMenu> entriesInitializer)
    {
        _entriesFactory = entriesInitializer.InitializeEntries();
    }

    /// <summary>
    /// Creates a menu command based on the provided entry from a menu command factory.
    /// </summary>
    /// <typeparam name="TMenu">The type of the menu entry</typeparam>
    /// <param name="entry">The entry from the menu</param>
    /// <returns>An ICommand instance based on the provided entry</returns>
    public ICommand Create(TMenu entry)
    {
        if (_entriesFactory.TryGetValue(entry, out var factory))
        {
            return factory();
        }

        throw new InvalidOperationException($"No factory found for the {entry}");
    }
}