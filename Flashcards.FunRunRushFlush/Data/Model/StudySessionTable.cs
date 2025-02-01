namespace Flashcards.FunRunRushFlush.Data.Model;

public class StudySession
{
    public StudySession() { }
    public StudySession(long id, long stackId, string stackName, string usedFlashcards, DateTime date)
    {
        Id = id;
        StackId = stackId;
        StackName = stackName;
        UsedFlashcards = usedFlashcards;
        Date = date;
    }

    public long Id { get; set; }
    public long StackId { get; set; }
    public string StackName { get; set; }
    public string UsedFlashcards { get; set; }
    public DateTime Date { get; set; }


}


public static class StudySessionTable
{
    public const string TableCreateStatment = """
        IF NOT EXISTS (
            SELECT 1 
            FROM sys.tables 
            WHERE name = 'StudySession' AND schema_id = SCHEMA_ID('dbo')
        )
        BEGIN
            CREATE TABLE StudySession (
                Id BIGINT PRIMARY KEY IDENTITY(1,1),
                StackId BIGINT NOT NULL,
                UsedFlashcards NVARCHAR(MAX) NOT NULL,
                Date DATETIME NOT NULL,
                CONSTRAINT FK_StudySession_Stack FOREIGN KEY (StackId) REFERENCES Stack(Id) ON DELETE CASCADE
            )
        END;
        """;


    public const string TableName = "StudySession";

    public const string Id = "Id";
    public const string StackId = "StackId";
    public const string UsedFlashcards = "UsedFlashcards";
    public const string Date = "Date";

}