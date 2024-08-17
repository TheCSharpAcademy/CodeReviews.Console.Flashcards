namespace Flashcards.Interfaces.Repositories.Operations;

/// <summary>
/// Represents an interface for retrieving all entities from a repository.
/// </summary>
/// <typeparam name="TEntity">The type of entities to retrieve.</typeparam>
internal interface IGetAllFromRepository<out TEntity>
{
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="TEntity"/> from the repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to retrieve.</typeparam>
    /// <returns>All entities of type <typeparamref name="TEntity"/>.</returns>
    internal IEnumerable<TEntity> GetAll();
}