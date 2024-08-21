using Flashcards.Eddyfadeev.Interfaces.Models;

namespace Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

/// <summary>
/// Represents an interface for updating entities in a repository.
/// </summary>
internal interface IUpdateInRepository<in TEntity>
{
    /// <summary>
    /// Updates the selected entry in the repository.
    /// </summary>
    /// <returns>The number of affected rows.</returns>
    internal int Update(IDbEntity<TEntity> entity);
}