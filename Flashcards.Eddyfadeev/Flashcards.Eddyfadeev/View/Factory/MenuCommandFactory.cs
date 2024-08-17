using Flashcards.Interfaces.View.Commands;
using Flashcards.Interfaces.View.Factory;

namespace Flashcards.View.Factory;

/// <summary>
/// Represents a factory for creating menu commands.
/// </summary>
/// <typeparam name="TMenu">The type of menu entries.</typeparam>
internal class MenuCommandFactory<TMenu> : IMenuCommandFactory<TMenu> where TMenu : Enum
{
    private readonly Dictionary<TMenu, Func<ICommand>> _entriesFactory;

    /// The MenuCommandFactory class is responsible for creating an instance of the ICommand interface based on the provided menu entry.
    /// @param <TMenu> The type of the menu entry enum.
    /// /
    public MenuCommandFactory(IMenuEntriesInitializer<TMenu> entriesInitializer)
    {
        _entriesFactory = entriesInitializer.InitializeEntries(this);
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