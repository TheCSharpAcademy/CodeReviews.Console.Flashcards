using Flashcards.Eddyfadeev.Interfaces.Models;

namespace Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

/// Represents an interface for deleting entries from a repository.
internal interface IDeleteFromRepository<in TEntity>
{
    /// <summary>
    /// Deletes an entry from the repository.
    /// </summary>
    /// <returns>The number of entries deleted from the repository.</returns>
    internal int Delete(IDbEntity<TEntity> entity);
}