USE [master];
GO



PRINT N'Creating database Flashcards...'
GO
CREATE DATABASE [Flashcards]
GO



USE [Flashcards];
GO



PRINT N'Creating Table [dbo].[Flashcard]...';
GO
CREATE TABLE [dbo].[Flashcard] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [StackId]  INT            NOT NULL,
    [Question] NVARCHAR (256) NOT NULL,
    [Answer]   NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Flashcard] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO



PRINT N'Creating Index [dbo].[Flashcard].[IX_Flashcard_StackId]...';
GO
CREATE NONCLUSTERED INDEX [IX_Flashcard_StackId]
    ON [dbo].[Flashcard]([StackId] ASC);
GO



PRINT N'Creating Table [dbo].[Stack]...';
GO
CREATE TABLE [dbo].[Stack] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_Stack] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Stack_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);
GO



PRINT N'Creating Table [dbo].[StudySession]...';
GO
CREATE TABLE [dbo].[StudySession] (
    [Id]       INT      IDENTITY (1, 1) NOT NULL,
    [StackId]  INT      NOT NULL,
    [DateTime] DATETIME NOT NULL,
    [Score]    INT      NOT NULL,
    CONSTRAINT [PK_StudySession] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO



PRINT N'Creating Index [dbo].[StudySession].[IX_StudySession_StackId]...';
GO
CREATE NONCLUSTERED INDEX [IX_StudySession_StackId]
    ON [dbo].[StudySession]([StackId] ASC);
GO



PRINT N'Creating Foreign Key [dbo].[FK_Flashcard_Stack]...';
GO
ALTER TABLE [dbo].[Flashcard]
    ADD CONSTRAINT [FK_Flashcard_Stack] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stack] ([Id]) ON DELETE CASCADE;


GO



PRINT N'Creating Foreign Key [dbo].[FK_StudySession_Stack]...';
GO
ALTER TABLE [dbo].[StudySession]
    ADD CONSTRAINT [FK_StudySession_Stack] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stack] ([Id]) ON DELETE CASCADE;


GO



PRINT N'Creating View [dbo].[vwAverageStudySessionScoreReport]...';
GO
CREATE VIEW [dbo].[vwAverageStudySessionScoreReport] 
AS
	SELECT
		 [StackName]
		,[StudyYear]
		,COALESCE([January], 0) AS [January]
		,COALESCE([February], 0) AS [February]
		,COALESCE([March], 0) AS [March]
		,COALESCE([April], 0) AS [April]
		,COALESCE([May], 0) AS [May]
		,COALESCE([June], 0) AS [June]
		,COALESCE([July], 0) AS [July]
		,COALESCE([August], 0) AS [August]
		,COALESCE([September], 0) AS [September]
		,COALESCE([October], 0) AS [October]
		,COALESCE([November], 0) AS [November]
		,COALESCE([December], 0) AS [December]
	FROM
	(
		SELECT
			 st.[Name] AS [StackName]
			,DATENAME(year, ss.[DateTime]) AS [StudyYear]
			,DATENAME(month, ss.[DateTime]) AS [StudyMonth]
			,ss.[Score]
		FROM
			[dbo].[Stack]			AS st JOIN
			[dbo].[StudySession]	AS ss ON st.[Id] = ss.[StackId]
	) AS s
	PIVOT
	(
		AVG([Score])
		FOR [StudyMonth] IN ([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])
	) AS p
GO



PRINT N'Creating View [dbo].[vwTotalStudySessionsReport]...';
GO
CREATE VIEW [dbo].[vwTotalStudySessionsReport]
AS
	SELECT
		 [StackName]
		,[StudyYear]
		,[January]
		,[February]
		,[March]
		,[April]
		,[May]
		,[June]
		,[July]
		,[August]
		,[September]
		,[October]
		,[November]
		,[December]
	FROM
	(
		SELECT
			 st.[Name] AS [StackName]
			,DATENAME(year, ss.[DateTime]) AS [StudyYear]
			,DATENAME(month, ss.[DateTime]) AS [StudyMonth]
		FROM
			[dbo].[Stack]			AS st JOIN
			[dbo].[StudySession]	AS ss ON st.[Id] = ss.[StackId]
	) AS s
	PIVOT
	(
		COUNT([StudyMonth])
		FOR [StudyMonth] IN ([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])
	) AS p
GO



PRINT N'Creating Function [dbo].[fnGetStackId]...';
GO
CREATE FUNCTION [dbo].[fnGetStackId]
(
	@Name	NVARCHAR(64)
)
RETURNS INT
AS
BEGIN

	DECLARE @Id INT

	SET @Id =
	(
		SELECT TOP 1
			[Id]
		FROM
			[dbo].[Stack]
		WHERE
			[Name] = @Name
	)

	RETURN @Id

END
GO



PRINT N'Creating Procedure [dbo].[AddFlashcard]...';
GO
CREATE PROCEDURE [dbo].[AddFlashcard]

	 @StackId	INT
	,@Question	NVARCHAR(256)
	,@Answer	NVARCHAR(256)

AS
BEGIN

	INSERT INTO [dbo].[Flashcard] 
	(
		 [StackId]
		,[Question]
		,[Answer]
	)
	VALUES
	(
		 @StackId
		,@Question
		,@Answer
	)

END
GO



PRINT N'Creating Procedure [dbo].[AddStack]...';
GO
CREATE PROCEDURE [dbo].[AddStack]

	@Name NVARCHAR(64)

AS
BEGIN

	INSERT INTO [dbo].[Stack] 
	(
		[Name]
	)
	VALUES
	(
		@Name
	)

END
GO



PRINT N'Creating Procedure [dbo].[AddStudySession]...';
GO
CREATE PROCEDURE [dbo].[AddStudySession]

	 @StackId	INT
	,@DateTime	DATETIME
	,@Score		INT

AS
BEGIN

	INSERT INTO [dbo].[StudySession]
	(
		 [StackId]
		,[DateTime]
		,[Score]
	)
	VALUES
	(
		 @StackId
		,@DateTime
		,@Score
	)

END
GO



PRINT N'Creating Procedure [dbo].[DeleteFlashcard]...';
GO
CREATE PROCEDURE [dbo].[DeleteFlashcard]

	@Id	INT

AS
BEGIN

	DELETE FROM 
		[dbo].[Flashcard] 
	WHERE
		[Id] = @Id

END
GO



PRINT N'Creating Procedure [dbo].[DeleteStack]...';
GO
CREATE PROCEDURE [dbo].[DeleteStack]
	
	@Id	INT

AS
BEGIN

	DELETE FROM
		[dbo].[Stack]
	WHERE
		[Id] = @Id

END
GO



PRINT N'Creating Procedure [dbo].[GetAverageStudySessionScoreReportByYear]...';
GO
CREATE PROCEDURE [dbo].[GetAverageStudySessionScoreReportByYear]

	 @Year	NVARCHAR(4)

AS
BEGIN

	SELECT
		*
	FROM
		[dbo].[vwAverageStudySessionScoreReport]
	WHERE
		[StudyYear] = @Year
	ORDER BY
		[StackName]

END
GO



PRINT N'Creating Procedure [dbo].[GetFlashcard]...';
GO
CREATE PROCEDURE [dbo].[GetFlashcard]

	@Id	INT

AS
BEGIN

	SELECT TOP 1
		*
	FROM 
		[dbo].[Flashcard] 
	WHERE
		[Id] = @Id

END
GO



PRINT N'Creating Procedure [dbo].[GetFlashcards]...';
GO
CREATE PROCEDURE [dbo].[GetFlashcards]

AS
BEGIN

	SELECT
		*
	FROM 
		[dbo].[Flashcard] 

END
GO



PRINT N'Creating Procedure [dbo].[GetFlashcardsByStackId]...';
GO
CREATE PROCEDURE [dbo].[GetFlashcardsByStackId]

	@StackId	INT

AS
BEGIN

	SELECT
		*
	FROM 
		[dbo].[Flashcard] 
	WHERE
		[StackId] = @StackId

END
GO



PRINT N'Creating Procedure [dbo].[GetStack]...';
GO
CREATE PROCEDURE [dbo].[GetStack]
	
	@Id	INT

AS
BEGIN

	SELECT TOP 1
		*
	FROM
		[dbo].[Stack]
	WHERE
		[Id] = @Id

END
GO



PRINT N'Creating Procedure [dbo].[GetStackByName]...';
GO
CREATE PROCEDURE [dbo].[GetStackByName]
	
	@Name	NVARCHAR(64)

AS
BEGIN

	SELECT
		*
	FROM
		[dbo].[Stack]
	WHERE
		[Id] = [dbo].[fnGetStackId](@Name)

END
GO



PRINT N'Creating Procedure [dbo].[GetStacks]...';
GO
CREATE PROCEDURE [dbo].[GetStacks]
	
AS
BEGIN

	SELECT
		*
	FROM
		[dbo].[Stack]

END
GO



PRINT N'Creating Procedure [dbo].[GetStudySessions]...';
GO
CREATE PROCEDURE [dbo].[GetStudySessions]

AS
BEGIN

	SELECT
		*
	FROM
		[dbo].[StudySession]

END
GO



PRINT N'Creating Procedure [dbo].[GetTotalStudySessionsReportByYear]...';
GO
CREATE PROCEDURE [dbo].[GetTotalStudySessionsReportByYear]

	 @Year	NVARCHAR(4)

AS
BEGIN

	SELECT
		*
	FROM
		[dbo].[vwTotalStudySessionsReport]
	WHERE
		[StudyYear] = @Year
	ORDER BY
		[StackName]

END
GO



PRINT N'Creating Procedure [dbo].[SetFlashcard]...';
GO
CREATE PROCEDURE [dbo].[SetFlashcard]

	 @Id		INT
	,@Question	NVARCHAR(256)
	,@Answer	NVARCHAR(256)

AS
BEGIN

	UPDATE
		[dbo].[Flashcard] 
	SET
		 [Question] = @Question
		,[Answer] = @Answer
	WHERE
		[Id] = @Id

END
GO



PRINT N'Creating Procedure [dbo].[SetStack]...';
GO
CREATE PROCEDURE [dbo].[SetStack]

	 @Id	INT
	,@Name	NVARCHAR(64)

AS
BEGIN

	UPDATE 
		[dbo].[Stack]
	SET
		[Name] =  @Name
	WHERE
		[Id] = @Id
	
END
GO



PRINT N'Script complete.';
GO
