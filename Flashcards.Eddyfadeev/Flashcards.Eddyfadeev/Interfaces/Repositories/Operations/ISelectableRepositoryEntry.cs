namespace Flashcards.Interfaces.Repositories.Operations;

/// <summary>
/// Represents a repository entry that can be selected.
/// </summary>
/// <typeparam name="TEntity">The type of the selected entry.</typeparam>
internal interface ISelectableRepositoryEntry<TEntity>
{
    /// <summary>
    /// Gets or sets the selected entry.
    /// </summary>
    /// <typeparam name="TEntity">The type of the selected entry.</typeparam>
    /// <value>The selected entry.</value>
    /// <remarks>
    /// Example usage:
    /// <code>
    /// var selectedEntry = repository.SelectedEntry;
    /// repository.SelectedEntry = newEntry;
    /// </code>
    /// </remarks>
    internal TEntity? SelectedEntry { get; set; }
}