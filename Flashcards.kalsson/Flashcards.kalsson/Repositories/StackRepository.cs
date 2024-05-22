using System.Data;
using Dapper;
using Flashcards.kalsson.DTOs;
using Flashcards.kalsson.Interfaces;
using Flashcards.kalsson.Models;

namespace Flashcards.kalsson.Repositories;

public class StackRepository : IStackRepository
{
    private readonly DatabaseConfig _dbConfig;

    /// <summary>
    /// Represents a repository for managing stacks.
    /// </summary>
    public StackRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    /// <summary>
    /// Retrieves all stacks from the database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of StackDTO objects.</returns>
    /// <remarks>
    /// This method queries the database for all stack records and maps them to StackDTO objects.
    /// </remarks>
    public async Task<IEnumerable<StackDTO>> GetAllStacksAsync()
    {
        using (IDbConnection connection = _dbConfig.NewConnection)
        {
            var stacks = await connection.QueryAsync<Stack>("SELECT * FROM Stacks");
            return stacks.Select(s => new StackDTO
            {
                StackId = s.StackId,
                Name = s.Name
            }).ToList();
        }
    }

    /// <summary>
    /// Adds a new stack to the database asynchronously.
    /// </summary>
    /// <param name="stack">The stack to be added.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the stack ID of the newly added stack.</returns>
    /// <remarks>
    /// This method inserts a new record into the Stacks table with the specified name and retrieves the auto-generated stack ID.
    /// </remarks>
    public async Task<int> AddStackAsync(StackDTO stack)
    {
        using (IDbConnection connection = _dbConfig.NewConnection)
        {
            var sql = "INSERT INTO Stacks (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.QuerySingleAsync<int>(sql, new { stack.Name });
        }
    }

    /// <summary>
    /// Deletes a stack from the database asynchronously.
    /// </summary>
    /// <param name="stackId">The ID of the stack to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method deletes the stack with the specified ID from the Stacks table in the database.
    /// </remarks>
    public async Task DeleteStackAsync(int stackId)
    {
        using (IDbConnection connection = _dbConfig.NewConnection)
        {
            await connection.ExecuteAsync("DELETE FROM Stacks WHERE StackId = @StackId", new { StackId = stackId });
        }
    }
}