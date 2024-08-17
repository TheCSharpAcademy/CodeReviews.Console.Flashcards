namespace Flashcards.Interfaces.Repositories.Operations;

/// Represents an interface for deleting entries from a repository.
internal interface IDeleteFromRepository
{
    /// <summary>
    /// Deletes an entry from the repository.
    /// </summary>
    /// <returns>The number of entries deleted from the repository.</returns>
    internal int Delete();
}