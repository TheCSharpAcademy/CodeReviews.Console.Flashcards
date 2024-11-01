SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flashcards](
	[FlashcardId] [int] IDENTITY(1,1) NOT NULL,
	[StackId] [int] NOT NULL,
	[Question] [nvarchar](250) NOT NULL,
	[Answer] [nvarchar](250) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Flashcards] ADD PRIMARY KEY CLUSTERED
(
	[FlashcardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Flashcards] ADD UNIQUE NONCLUSTERED
(
	[Question] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Flashcards]  WITH CHECK ADD  CONSTRAINT [FK_StackId] FOREIGN KEY([StackId])
REFERENCES [dbo].[Stacks] ([StackId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Flashcards] CHECK CONSTRAINT [FK_StackId]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stacks](
	[StackId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Stacks] ADD PRIMARY KEY CLUSTERED
(
	[StackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Stacks] ADD UNIQUE NONCLUSTERED
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudySessions](
	[StudySessionId] [int] IDENTITY(1,1) NOT NULL,
	[StackId] [int] NOT NULL,
	[Score] [int] NOT NULL,
	[CurrentDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudySessions] ADD PRIMARY KEY CLUSTERED
(
	[StudySessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudySessions]  WITH CHECK ADD  CONSTRAINT [FK_StudySession_StackId] FOREIGN KEY([StackId])
REFERENCES [dbo].[Stacks] ([StackId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudySessions] CHECK CONSTRAINT [FK_StudySession_StackId]
GO

