using Dapper;
using Flashcards.kalsson.DTOs;
using Flashcards.kalsson.Interfaces;
using Flashcards.kalsson.Models;

namespace Flashcards.kalsson.Repositories;

public class FlashcardRepository : IFlashcardRepository
{
    private readonly DatabaseConfig _dbConfig;

    public FlashcardRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    /// <summary>
    /// Retrieves a collection of flashcards asynchronously by the given stack ID.
    /// </summary>
    /// <param name="stackId">The ID of the stack to retrieve flashcards from.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a collection of <see cref="FlashcardDTO"/> objects representing the retrieved flashcards.</returns>
    public async Task<IEnumerable<FlashcardDTO>> GetFlashcardsByStackIdAsync(int stackId)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            var flashcards = await connection.QueryAsync<Flashcard>(
                "SELECT FlashcardId, Content FROM Flashcards WHERE StackId = @StackId ORDER BY FlashcardId", 
                new { StackId = stackId });

            // Renumber the FlashcardId for display purposes
            int displayId = 1;
            var flashcardsForDisplay = flashcards.Select(fc => new FlashcardDTO
            {
                FlashcardId = displayId++,
                Content = fc.Content
            }).ToList();

            return flashcardsForDisplay;
        }
    }

    /// <summary>
    /// Adds a flashcard asynchronously.
    /// </summary>
    /// <param name="flashcard">The <see cref="FlashcardDTO"/> object representing the flashcard to be added.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains an integer representing the ID of the added flashcard.</returns>
    public async Task<int> AddFlashcardAsync(FlashcardDTO flashcard)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            var sql = "INSERT INTO Flashcards (Content, StackId) VALUES (@Content, @StackId); SELECT CAST(SCOPE_IDENTITY() as int);";
            // Assume StackId is somehow resolved before calling this method
            return await connection.QuerySingleAsync<int>(sql, new { flashcard.Content, StackId = ResolveStackId() });
        }
    }

    /// <summary>
    /// Deletes a flashcard asynchronously.
    /// </summary>
    /// <param name="flashcardId">The ID of the flashcard to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DeleteFlashcardAsync(int flashcardId)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            await connection.ExecuteAsync("DELETE FROM Flashcards WHERE FlashcardId = @FlashcardId", new { FlashcardId = flashcardId });
        }
    }

    /// <summary>
    /// Resolves the ID of the stack to be used when adding a flashcard asynchronously.
    /// </summary>
    /// <returns>The resolved stack ID.</returns>
    private int ResolveStackId()
    {
        // Logic to resolve StackId
        return 1; // Placeholder
    }
}