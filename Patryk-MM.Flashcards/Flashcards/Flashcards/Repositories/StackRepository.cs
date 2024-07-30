using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Flashcards.Repositories;

/// <summary>
/// Provides data access methods for managing <see cref="Stack"/> entities.
/// </summary>
public class StackRepository : BaseRepository<Stack>, IStackRepository {
    /// <summary>
    /// Initializes a new instance of the <see cref="StackRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to interact with the database.</param>
    public StackRepository(AppDbContext dbContext) : base(dbContext) {
    }

    /// <summary>
    /// Retrieves a list of stack names, optionally filtered by a predicate.
    /// </summary>
    /// <param name="predicate">An optional expression defining the condition that stacks must satisfy.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of stack names.</returns>
    public async Task<List<string>> GetStackNamesAsync(Expression<Func<Stack, bool>>? predicate = null) {
        if (predicate == null) {
            return await DbSet
                .Select(x => x.Name)
                .ToListAsync();
        }

        return await DbSet
            .Where(predicate)
            .Select(x => x.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a <see cref="Stack"/> by its unique identifier, including its flashcards.
    /// </summary>
    /// <param name="id">The unique identifier of the stack.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the stack with its flashcards.</returns>
    public async Task<Stack> GetStackByIdAsync(int id) {
        return await DbSet
            .Include(x => x.Flashcards)
            .FirstAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves a <see cref="Stack"/> by its name, including its flashcards.
    /// </summary>
    /// <param name="name">The name of the stack.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the stack with its flashcards if found; otherwise, <c>null</c>.</returns>
    public async Task<Stack?> GetStackByNameAsync(string name) {
        return await DbSet
            .Include(x => x.Flashcards)
            .FirstOrDefaultAsync(x => x.Name == name);
    }
}
