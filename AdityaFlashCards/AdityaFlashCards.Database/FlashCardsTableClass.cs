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

    internal void UpdateFlashCard(int fK_StackID, int positionInStack, string question, string answer) 
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("UPDATE FlashCards SET Question = @question, Answer = @answer WHERE Fk_StackID = @fK_StackID AND PositionInStack = @positionInStack", new { question, answer, fK_StackID, positionInStack });
    }
}

