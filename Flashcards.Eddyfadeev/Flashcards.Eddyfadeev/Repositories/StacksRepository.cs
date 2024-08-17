using Flashcards.Extensions;
using Flashcards.Interfaces.Database;
using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Repositories;
using Flashcards.Models.Dto;
using Flashcards.Services;

namespace Flashcards.Repositories;

/// <summary>
/// Represents a repository for managing stacks of flashcards.
/// </summary>
internal class StacksRepository : IStacksRepository
{
    private readonly IDatabaseManager _databaseManager;

    public IStack? SelectedEntry { get; set; }
    
    public StacksRepository(IDatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Inserts an entity into the Stacks table.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to insert.</typeparam>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The number of rows affected.</returns>
    public int Insert(IDbEntity<IStack> entity)
    {
        var stack = entity.MapToDto();
        const string query = "INSERT INTO Stacks (Name) VALUES (@Name);";

        return _databaseManager.InsertEntity(query, stack);
    }

    /// <summary>
    /// Deletes an entry from the repository.
    /// </summary>
    /// <returns>The number of entries deleted from the repository.</returns>
    public int Delete()
    {
        const string deleteQuery = "DELETE FROM Stacks WHERE Id = @Id;";
        
        var parameters = new { SelectedEntry.Id };
        
        return _databaseManager.DeleteEntry(deleteQuery, parameters);
    }

    /// <summary>
    /// Updates the selected entry in the repository.
    /// </summary>
    /// <returns>The number of affected rows</returns>
    public int Update()
    {
        if (GeneralHelperService.CheckForNull(SelectedEntry))
        {
            return 0;
        }

        var stack = SelectedEntry.ToDto();
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
}