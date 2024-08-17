using Flashcards.Eddyfadeev.Interfaces.Models;

namespace Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

/// <summary>
/// Represents an interface for inserting entities into a repository.
/// </summary>
/// <typeparam name="TEntity">The type of entity to insert.</typeparam>
internal interface IInsertIntoRepository<in TEntity>
{
    /// <summary>
    /// Inserts the specified entity into the database.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be inserted.</param>
    /// <returns>The number of rows affected in the database.</returns>
    internal int Insert(IDbEntity<TEntity> entity);
}