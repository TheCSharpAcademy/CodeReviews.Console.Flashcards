namespace Flashcards.Interfaces.Models;

/// <summary>
/// Represents an entity that can be stored in a database.
/// </summary>
/// <typeparam name="TEntity">The type of DTO (Data Transfer Object) that represents the entity.</typeparam>
internal interface IDbEntity<out TEntity>
{
    /// <summary>
    /// Maps an object implementing the <see cref="IDbEntity{T}"/> interface to a DTO object.
    /// </summary>
    /// <typeparam name="TEntity">The type of the DTO.</typeparam>
    /// <returns>The DTO object.</returns>
    internal TEntity MapToDto();
}