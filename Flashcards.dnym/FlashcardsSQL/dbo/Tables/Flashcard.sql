CREATE TABLE [dbo].[Flashcard]
(
	[FlashcardId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StackId] INT NOT NULL, 
    [Front] NVARCHAR(MAX) NOT NULL, 
    [Back] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [Flashcard_BelongsTo_Stack_fk] FOREIGN KEY ([StackId]) REFERENCES [Stack]([StackId]) ON DELETE CASCADE
)
