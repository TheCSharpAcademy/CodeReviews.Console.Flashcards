using Dapper;
using FlashcardsProgram.Database;

namespace FlashcardsProgram.Flashcards;

public class FlashcardsRepository(string tableName) : BaseRepository<FlashcardDAO>(tableName)
{
    public int GetNumCardsInStack(int stackId)
    {
        string sql = $@"
            SELECT COUNT(*) as 'Count' FROM Flashcards
            WHERE StackId = @StackId
        ";

        var result = ConnectionManager.Connection.QuerySingle<CountQueryResult>(sql, new { StackId = stackId });
        return result.Count;
    }
}

class CountQueryResult
{
    public int Count;
}