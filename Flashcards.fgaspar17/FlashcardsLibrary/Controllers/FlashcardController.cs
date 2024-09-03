using System.Data.SqlClient;
using System.Data;
using CodingTrackerLibrary;
using Dapper;

namespace FlashcardsLibrary;
public class FlashcardController
{
    public static List<Flashcard> GetFlashcardsByStackId(int stackId)
    {
        string sql = " SELECT ROW_NUMBER() OVER(ORDER BY FlashcardId) AS OrderId, FlashcardId, StackId, Question, Answer FROM Flashcards WHERE StackId = @StackId";
        return SqlExecutionService.GetListModelsByKey<int, Flashcard>(sql, field: "StackId", stackId);
    }

    public static Flashcard GetFlashcardById(int id)
    {
        string sql = " SELECT ROW_NUMBER() OVER(ORDER BY FlashcardId) AS OrderId, FlashcardId, StackId, Question, Answer FROM Flashcards WHERE FlashcardId = @FlashcardId";
        return SqlExecutionService.GetModelByKey<int, Flashcard>(sql, field: "FlashcardId", id);
    }

    public static int? GetFlashcardIdByOrderId(int stackId, int orderId)
    {
        string sql = @"
                        SELECT FlashcardId
                        FROM (
                            SELECT ROW_NUMBER() OVER(ORDER BY FlashcardId) AS OrderId, FlashcardId
                            FROM Flashcards
                            WHERE StackId = @StackId
                        ) AS OrderedFlashcards
                        WHERE OrderId = @OrderId";

        using IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString);
        connection.Open();
        int? flashcardId = connection.QuerySingleOrDefault<int>(sql, new { StackId = stackId, OrderId = orderId });
        connection.Close();

        return flashcardId;
    }


    public static bool InsertFlashcard(Flashcard flashcard)
    {
        string sql = $@"INSERT INTO dbo.Flashcards (StackId, Question, Answer) 
                                        VALUES (@StackId, @Question, @Answer);";

        return SqlExecutionService.ExecuteCommand<Flashcard>(sql, flashcard);
    }

    public static bool UpdateFlashcard(Flashcard flashcard)
    {
        string sql = $@"UPDATE dbo.Flashcards SET
                                StackId = @StackId,
                                Question = @Question,
                                Answer = @Answer
                                WHERE FlashcardId = @FlashcardId;";

        return SqlExecutionService.ExecuteCommand<Flashcard>(sql, flashcard);
    }

    public static bool DeleteFlashcard(Flashcard flashcard)
    {
        string sql = $@"DELETE FROM dbo.Flashcards
                                WHERE FlashcardId = @FlashcardId;";

        return SqlExecutionService.ExecuteCommand<Flashcard>(sql, flashcard);
    }
}