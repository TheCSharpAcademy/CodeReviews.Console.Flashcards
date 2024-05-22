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

    public async Task<IEnumerable<FlashcardDTO>> GetFlashcardsByStackIdAsync(int stackId)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            var flashcards = await connection.QueryAsync<Flashcard>(
                "SELECT * FROM Flashcards WHERE StackId = @StackId", new { StackId = stackId });
            return flashcards.Select(fc => new FlashcardDTO
            {
                FlashcardId = fc.FlashcardId,
                Content = fc.Content
                // Notice StackId is not included
            }).ToList();
        }
    }

    public async Task<int> AddFlashcardAsync(FlashcardDTO flashcard)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            var sql = "INSERT INTO Flashcards (Content, StackId) VALUES (@Content, @StackId); SELECT CAST(SCOPE_IDENTITY() as int);";
            // Assume StackId is somehow resolved before calling this method
            return await connection.QuerySingleAsync<int>(sql, new { flashcard.Content, StackId = ResolveStackId() });
        }
    }

    public async Task DeleteFlashcardAsync(int flashcardId)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            await connection.ExecuteAsync("DELETE FROM Flashcards WHERE FlashcardId = @FlashcardId", new { FlashcardId = flashcardId });
        }
    }

    private int ResolveStackId()
    {
        // Logic to resolve StackId
        return 1; // Placeholder
    }
}