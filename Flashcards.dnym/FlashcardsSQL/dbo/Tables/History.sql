CREATE TABLE [dbo].[History]
(
	[HistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StackId] INT NOT NULL, 
    [StartedAt] DATETIME2 NOT NULL, 
    CONSTRAINT [History_PertainsTo_Stack_fk] FOREIGN KEY ([StackId]) REFERENCES [Stack]([StackId]) ON DELETE CASCADE
)

GO

CREATE TRIGGER [dbo].[Delete_StudyResults_trg]
    ON [dbo].[History]
    FOR DELETE
    AS
    BEGIN
        SET NOCOUNT ON;
        DELETE FROM [dbo].[StudyResult]
		WHERE [HistoryId] IN (SELECT [HistoryId] FROM DELETED);
    END