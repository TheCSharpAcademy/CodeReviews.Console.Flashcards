CREATE TABLE [dbo].[Flashcard]
(
	 [Id] INT NOT NULL IDENTITY
    ,[StackId] INT NOT NULL
    ,[Question] NVARCHAR(256) NOT NULL
    ,[Answer] NVARCHAR(256) NOT NULL
    ,CONSTRAINT [PK_Flashcard] PRIMARY KEY ([Id])
    ,CONSTRAINT [FK_Flashcard_Stack] FOREIGN KEY ([StackId]) REFERENCES [Stack]([Id]) ON DELETE CASCADE
)
GO
CREATE NONCLUSTERED INDEX [IX_Flashcard_StackId] ON [dbo].[Flashcard] ([StackId])
GO
