using Dapper;
using FlashcardsProgram.Database;

namespace FlashcardsProgram.Flashcards;

public class FlashcardsRepository(string tableName) : BaseRepository<FlashcardDao>(tableName)
{
    public List<FlashcardDao> GetByStackId(int stackId)
    {
        string sql = $@"
            SELECT * FROM Flashcards
            WHERE StackId = @StackId
        ";

        return ConnectionManager.Connection
            .Query<FlashcardDao>(sql, new { StackId = stackId }).ToList();
    }

    public int GetNumCardsInStack(int stackId)
    {
        string sql = $@"
            SELECT COUNT(*) as 'Count' FROM Flashcards
            WHERE StackId = @StackId
        ";

        var result = ConnectionManager.Connection
            .QuerySingle<CountQueryResult>(sql, new { StackId = stackId });
        return result.Count;
    }
}

internal class CountQueryResult(int count)
{
    public int Count = count;
}