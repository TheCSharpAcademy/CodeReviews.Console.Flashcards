namespace FunRun.Flashcards.Data.Model;

public class Flashcards
{
    public long Id { get; set; }
    public long StackId { get; set; }
    public required string Front { get; set; }
    public required string Back { get; set; }
    public bool Solved { get; set; }
}

public static class FlashcardsTable
{
public const string TableCreateStatment = """
    IF NOT EXISTS (
        SELECT 1 
        FROM sys.tables 
        WHERE name = 'Flashcards' AND schema_id = SCHEMA_ID('dbo')
    )
    BEGIN
        CREATE TABLE Flashcards (
            Id BIGINT PRIMARY KEY IDENTITY(1,1),
            StackId BIGINT NOT NULL,
            Front NVARCHAR(MAX) NOT NULL,
            Back NVARCHAR(MAX) NOT NULL,
            Solved BIT NOT NULL,
            CONSTRAINT FK_Flashcards_Stack FOREIGN KEY (StackId) REFERENCES StackTable(Id) ON DELETE CASCADE
        );
    END;
    
    """;

    public const string TableName = "Flashcards";
    public const string Id = "Id";
    public const string StackId = "StackId";
    public const string Front = "Front";
    public const string Back = "Back";
    public const string Solved = "Solved";
}