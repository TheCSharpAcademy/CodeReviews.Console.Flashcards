using Flashcards.Eddyfadeev.Interfaces.Database;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Models.Dto;
using Flashcards.Eddyfadeev.Services;
using Flashcards.Eddyfadeev.Extensions;

namespace Flashcards.Eddyfadeev.Repositories;

/// <summary>
/// Represents a repository for managing flashcards.
/// </summary>
internal class FlashcardsRepository : IFlashcardsRepository
{
    private readonly IDatabaseManager _databaseManager;
    
    public int? StackId { get; set; }
    public string? StackName { get; set; }
    public IFlashcard? SelectedEntry { get; set; }
    
    public FlashcardsRepository(IDatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Inserts a new entity into the Flashcards table in the database.
    /// </summary>
    /// <param name="entity">The <see cref="IDbEntity{TEntity}"/> object representing the entity to be inserted.</param>
    /// <returns>The number of rows affected by the insert operation.</returns>
    public int Insert(IDbEntity<IFlashcard> entity)
    {
        var stack = entity.MapToDto();
        
        const string query = "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId);";

        return _databaseManager.InsertEntity(query, stack);
    }

    /// <summary>
    /// Deletes the selected flashcard from the repository.
    /// </summary>
    /// <returns>The number of flashcards deleted from the repository.</returns>
    public int Delete()
    {
        if (GeneralHelperService.CheckForNull(SelectedEntry))
        {
            return 0;
        }
        
        var parameters = new { Id = SelectedEntry.Id };
        
        const string deleteQuery = "DELETE FROM Flashcards WHERE Id = @Id;";
        
        return _databaseManager.DeleteEntry(deleteQuery, parameters);
    }

    /// <summary>
    /// Updates the flashcard in the repository.
    /// </summary>
    /// <returns>The number of rows affected (should be 1 if successful, 0 otherwise).</returns>
    public int Update()
    {
        if (GeneralHelperService.CheckForNull(SelectedEntry))
        {
            return 0;
        }

        var flashcard = SelectedEntry.ToDto();
        
        const string query = "UPDATE Flashcards SET Question = @Question, Answer = @Answer WHERE Id = @Id;";
        
        return _databaseManager.UpdateEntry(query, flashcard);
    }

    /// <summary>
    /// Retrieves all the flashcards from the database that belong to a specific stack.
    /// </summary>
    /// <returns>
    /// An IEnumerable of IFlashcard representing all the flashcards in the stack.
    /// </returns>
    public IEnumerable<IFlashcard> GetAll()
    {
        const string query = "SELECT * FROM Flashcards WHERE StackId = @StackId;";
        object parameters = new
        {
            StackId
        };
        
        IEnumerable<IFlashcard> flashcards = _databaseManager.GetAllEntities<FlashcardDto>(query, parameters);
        
        flashcards = flashcards.Select(flashcard => flashcard.ToEntity());

        return flashcards;
    }
}