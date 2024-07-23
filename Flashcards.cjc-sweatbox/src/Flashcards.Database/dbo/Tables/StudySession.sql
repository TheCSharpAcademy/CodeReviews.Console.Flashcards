CREATE TABLE [dbo].[StudySession]
(
	 [Id] INT NOT NULL IDENTITY
    ,[StackId] INT NOT NULL
    ,[DateTime] DATETIME NOT NULL
    ,[Score] INT NOT NULL
    ,CONSTRAINT [PK_StudySession] PRIMARY KEY ([Id])
    ,CONSTRAINT [FK_StudySession_Stack] FOREIGN KEY ([StackId]) REFERENCES [Stack]([Id]) ON DELETE CASCADE
)
GO
CREATE NONCLUSTERED INDEX [IX_StudySession_StackId] ON [dbo].[StudySession] ([StackId])
GO
