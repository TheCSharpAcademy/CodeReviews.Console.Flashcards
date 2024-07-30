using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Flashcards.Repositories;

/// <summary>
/// Provides basic CRUD operations for entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of entity that the repository handles, which must inherit from <see cref="BaseEntity"/>.</typeparam>
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity {
    internal readonly AppDbContext DbContext;
    internal readonly DbSet<T> DbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to interact with the database.</param>
    public BaseRepository(AppDbContext dbContext) {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, <c>null</c>.</returns>
    public async Task<T?> GetByIdAsync(int id) {
        return await DbSet.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all entities, optionally including related entities specified by the <paramref name="includes"/> parameters.
    /// </summary>
    /// <param name="includes">Expressions specifying related entities to include in the query.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
    public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes) {
        IQueryable<T> query = DbSet;

        foreach (var include in includes) {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    /// <summary>
    /// Retrieves entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression defining the condition that entities must satisfy.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that match the predicate.</returns>
    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate) {
        return await DbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddAsync(T entity) {
        await DbSet.AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an existing entity from the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync(T entity) {
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task EditAsync(T entity) {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync();
    }
}
