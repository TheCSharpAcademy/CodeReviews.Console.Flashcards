using AdityaFlashCards.Database.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdityaFlashCards.Database;

internal class FlashCardsTableClass
{
    private string? _connectionString;

    public FlashCardsTableClass(string? connectionString)
    {
        _connectionString = connectionString;
    }

    internal void CreateTable()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute(@"CREATE TABLE FlashCards (FlashCardId INT IDENTITY(1,1) PRIMARY KEY, Question NVARCHAR(MAX) NOT NULL, Answer NVARCHAR(MAX) NOT NULL, FK_StackID INT NOT NULL FOREIGN KEY REFERENCES Stacks(StackID) ON DELETE CASCADE, PositionInStack INT NOT NULL, CONSTRAINT UC_FlashCards_StackId_PositionInStack UNIQUE (FK_StackID, PositionInStack))");
    }

    internal void UpdateFlashCard(int flashCardId, string question, string answer)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("UPDATE FlashCards SET Question = @question, Answer = @answer WHERE FlashCardId = @flashCardId", new { question, answer, flashCardId });
    }

    internal void DeleteFlashCard(int fK_StackID, int positionInStack)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("DELETE FROM FlashCards WHERE FK_StackID = @fk_StackID and PositionInStack = @positionInStack", new { fK_StackID, positionInStack });
        conn.Execute("UPDATE FlashCards SET PositionInStack = PositionInStack - 1 WHERE FK_StackID = @fk_StackID AND PositionInStack > @positionInStack", new {fK_StackID, positionInStack });
    }

    internal void InsertFlashCard(string question, string answer, int fK_StackID, int positionInStack)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("INSERT INTO FlashCards (Question, Answer, FK_StackID, PositionInStack) VALUES (@question, @answer, @fK_StackID, @positionInStack)", new { question, answer, fK_StackID, positionInStack });
    }

    internal (int fK_StackID, int positionInStack) GetStackIDAndPosition(int flashCardId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        string sql = "SELECT FK_StackID, PositionInStack FROM FlashCards WHERE FlashCardId = @FlashCardId";
        var result = conn.QuerySingleOrDefault<(int, int)>(sql, new { FlashCardId = flashCardId });
        return result;
    }

    internal List<FlashCardDTOFlashCardView> GetFlashCards()
    {
        List<FlashCardDTOFlashCardView> flashCards = new List<FlashCardDTOFlashCardView>();
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        string sql = "SELECT FlashCardId, Question, Answer FROM FlashCards ORDER BY FK_StackID";
        var result = conn.Query<FlashCardDTOFlashCardView>(sql);
        flashCards.AddRange(result);
        return flashCards;
    }

    internal List<FlashCardDTOStackView> GetFlashCardsForGivenStack(string name)
    {
        List<FlashCardDTOStackView> flashCards = new List<FlashCardDTOStackView>();
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        string sql = @"
        SELECT FlashCards.PositionInStack, FlashCards.Question, FlashCards.Answer 
        FROM FlashCards 
        WHERE FlashCards.FK_StackID = (
            SELECT Stacks.StackID 
            FROM Stacks 
            WHERE Stacks.Name = @name
        )
        ORDER BY FlashCards.PositionInStack";
        var result = conn.Query<FlashCardDTOStackView>(sql, new { name });
        flashCards.AddRange(result);
        return flashCards;
    }

    internal bool IsFlashCardIdPresent(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        string sql = "SELECT COUNT(*) FROM FlashCards WHERE FlashCardId = @id";
        int count = conn.QuerySingle<int>(sql, new { id });
        return count > 0;
    }

    internal int GetLastPositionInStack(int stackId)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            string sql = "SELECT MAX(PositionInStack) FROM FlashCards WHERE FK_StackID = @stackId";
            int lastPosition = conn.QueryFirstOrDefault<int>(sql, new { stackId });
            return lastPosition;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
}