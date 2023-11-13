CREATE TABLE [dbo].[StudyResult]
(
	[StudyResultId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HistoryId] INT NOT NULL, 
    [FlashcardId] INT NOT NULL, 
    [WasCorrect] BIT NOT NULL, 
    [AnsweredAt] DATETIME2 NOT NULL, 
    --Constraint changed to trigger in History.sql
    --CONSTRAINT [StudyResult_BelongsTo_History_fk] FOREIGN KEY ([HistoryId]) REFERENCES [History]([HistoryId]) ON DELETE CASCADE, 
    CONSTRAINT [StudyResult_PertainsTo_Flashcard_fk] FOREIGN KEY ([FlashcardId]) REFERENCES [Flashcard]([FlashcardId]) ON DELETE CASCADE
)
