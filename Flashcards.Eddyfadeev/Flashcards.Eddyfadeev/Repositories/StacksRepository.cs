using Flashcards.Eddyfadeev.Extensions;
using Flashcards.Eddyfadeev.Interfaces.Database;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Models.Dto;
using Flashcards.Eddyfadeev.Services;

namespace Flashcards.Eddyfadeev.Repositories;

/// <summary>
/// Represents a repository for managing stacks of flashcards.
/// </summary>
internal class StacksRepository : IStacksRepository
{
    private readonly IDatabaseManager _databaseManager;
    
    public StacksRepository(IDatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Inserts an entity into the Stacks table.
    /// </summary>
    /// <typeparam name="IStack">The type of entity to insert.</typeparam>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The number of rows affected.</returns>
    public int Insert(IDbEntity<IStack> entity)
    {
        if (GeneralHelperService.CheckForNull(entity))
        {
            return 0;
        }
        
        var stack = entity.MapToDto();
        const string query = "INSERT INTO Stacks (Name) VALUES (@Name);";

        return _databaseManager.InsertEntity(query, stack);
    }

    /// <summary>
    /// Deletes an entry from the repository.
    /// </summary>
    /// <returns>The number of entries deleted from the repository.</returns>
    public int Delete(IDbEntity<IStack> entity)
    {
        if (GeneralHelperService.CheckForNull(entity))
        {
            return 0;
        }
        
        var stack = entity.MapToDto();
        const string deleteQuery = "DELETE FROM Stacks WHERE Id = @Id;";
        
        return _databaseManager.DeleteEntry(deleteQuery, stack);
    }

    /// <summary>
    /// Updates the selected entry in the repository.
    /// </summary>
    /// <returns>The number of affected rows</returns>
    public int Update(IDbEntity<IStack> entity)
    {
        if (GeneralHelperService.CheckForNull(entity))
        {
            return 0;
        }

        var stack = entity.MapToDto();

        const string query = "UPDATE Stacks SET Name = @Name WHERE Id = @Id;";

        return _databaseManager.UpdateEntry(query, stack);
    }

    /// <summary>
    /// Retrieves all stacks from the repository.
    /// </summary>
    /// <returns>An enumerable collection of stack objects.</returns>
    public IEnumerable<IStack> GetAll()
    {
        const string query = "SELECT * FROM Stacks;";

        IEnumerable<IStack> stacks = _databaseManager.GetAllEntities<StackDto>(query);

        stacks = stacks.Select(stack => stack.ToEntity());

        return stacks;
    }
    
    /// <summary>
    /// Retrieves all stacks names from the repository.
    /// </summary>
    /// <returns>An enumerable collection of stack objects.</returns>
    public IEnumerable<IStack> GetStackNames()
    {
        const string query = "SELECT DISTINCT Id, Name as Name FROM Stacks;";
        
        IEnumerable<IStack> stackNames = _databaseManager.GetAllEntities<StackDto>(query);

        stackNames = stackNames.Select(stack => stack.ToEntity());

        return stackNames;
    }
}