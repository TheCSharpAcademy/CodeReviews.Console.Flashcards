
namespace FunRun.Flashcards.Data.Model;

public class Stack
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public static class StackTable
{
    public const string TableCreateStatment = """
        IF NOT EXISTS (
            SELECT 1 
            FROM sys.tables 
            WHERE name = 'Stack' AND schema_id = SCHEMA_ID('dbo')
        )
        BEGIN
            CREATE TABLE Stack (
                Id BIGINT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(255) NOT NULL UNIQUE
            );
        END;
        """;

    public const string TableName = "Stack";
    public const string Id = "Id";
    public const string Name = "Name"; //Unique
}
