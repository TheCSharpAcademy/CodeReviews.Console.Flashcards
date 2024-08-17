namespace Flashcards.Eddyfadeev.Interfaces.Handlers;

/// <summary>
/// Represents an interface for handling editable entries.
/// </summary>
/// <typeparam name="TEntry">The type of the entry.</typeparam>
internal interface IEditableEntryHandler<TEntry> where TEntry : class
{
    /// <summary>
    /// Handles an editable entry by presenting a list of entries and prompting the user to choose one.
    /// </summary>
    /// <typeparam name="TEntry">The type of entry.</typeparam>
    /// <param name="entries">The list of entries to choose from.</param>
    /// <returns>The selected entry, or null if no entry is selected.</returns>
    internal TEntry? HandleEditableEntry(List<TEntry> entries);
}