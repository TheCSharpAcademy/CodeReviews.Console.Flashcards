using Microsoft.Data.SqlClient;

namespace Flashcards.DataAccess;

public static class SqlServerPreparation
{
    public static void Prepare(string connectionString, bool withSampleData)
    {
		try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

			using (SqlCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = _createDatabase;
				cmd.ExecuteNonQuery();
			}

			foreach (var createTable in _createTables)
			{
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = createTable;
                    cmd.ExecuteNonQuery();
                }
            }

            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = _createTriggers;
                cmd.ExecuteNonQuery();
            }

			foreach (var createProcedure in _createProcedures)
			{
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = createProcedure;
                    cmd.ExecuteNonQuery();
                }
            }

			if (withSampleData)
            {
                using SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = _addSampleData;
                cmd.ExecuteNonQuery();
            }
        }
		catch (SqlException ex)
		{
			if (ex.Number == 1801)
			{
                //Database already exists
                return;
            }
            Console.Clear();
            Console.WriteLine($"Database error: {ex.Message}\n\nSuggestion: verify your connection string.\n\nAborting!");
            Environment.Exit(1);
        }
    }

    private const string _createDatabase = @"USE master;

CREATE DATABASE FlashcardsSQL;";

    private static readonly string[] _createTables = { "USE FlashcardsSQL;",
		@"CREATE TABLE [dbo].[Stack]
(
	[StackId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SortName] NVARCHAR(255) NOT NULL, 
    [ViewName] NVARCHAR(255) NOT NULL
);",
		@"CREATE TABLE [dbo].[Flashcard]
(
	[FlashcardId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StackId] INT NOT NULL, 
    [Front] NVARCHAR(MAX) NOT NULL, 
    [Back] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [Flashcard_BelongsTo_Stack_fk] FOREIGN KEY ([StackId]) REFERENCES [Stack]([StackId]) ON DELETE CASCADE
);",
		@"CREATE TABLE [dbo].[History]
(
	[HistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StackId] INT NOT NULL, 
    [StartedAt] DATETIME2 NOT NULL, 
    CONSTRAINT [History_PertainsTo_Stack_fk] FOREIGN KEY ([StackId]) REFERENCES [Stack]([StackId]) ON DELETE CASCADE
);",
		@"CREATE TABLE [dbo].[StudyResult]
(
	[StudyResultId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HistoryId] INT NOT NULL, 
    [FlashcardId] INT NOT NULL, 
    [WasCorrect] BIT NOT NULL, 
    [AnsweredAt] DATETIME2 NOT NULL, 
    --Constraint changed to trigger in History.sql
    --CONSTRAINT [StudyResult_BelongsTo_History_fk] FOREIGN KEY ([HistoryId]) REFERENCES [History]([HistoryId]) ON DELETE CASCADE, 
    CONSTRAINT [StudyResult_PertainsTo_Flashcard_fk] FOREIGN KEY ([FlashcardId]) REFERENCES [Flashcard]([FlashcardId]) ON DELETE CASCADE
);" };

	private const string _createTriggers = @"CREATE TRIGGER [dbo].[Delete_StudyResults_trg]
    ON [dbo].[History]
    FOR DELETE
    AS
    BEGIN
        SET NOCOUNT ON;
        DELETE FROM [dbo].[StudyResult]
		WHERE [HistoryId] IN (SELECT [HistoryId] FROM DELETED);
    END;";

    private static readonly string[] _createProcedures = {
		@"CREATE PROCEDURE [dbo].[Flashcard_Count_tr]
    @StackId INT = NULL
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT
			COUNT(F.FlashcardId) as Flashcards
		FROM Flashcard AS F
		WHERE F.StackId = @StackId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Flashcard_Create_tr]
	@StackId int,
	@Front nvarchar(MAX),
	@Back nvarchar(MAX)
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[Flashcard] ([StackId], [Front], [Back])
		VALUES (@StackId, @Front, @Back);
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Flashcard_Delete_tr]
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		DELETE FROM Flashcard WHERE FlashcardId = @FlashcardId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Flashcard_GetById_tr]
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT TOP 1
			F.FlashcardId, F.Front, F.Back
		FROM Flashcard AS F
		WHERE F.FlashcardId = @FlashcardId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Flashcard_GetMultiple_tr]
    @StackId INT,
    @Skip INT = NULL,
    @Take INT = NULL
AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

        SELECT F.FlashcardId, F.Front, F.Back
        FROM Flashcard AS F
        WHERE F.StackId = @StackId
        ORDER BY F.FlashcardId
        OFFSET ISNULL(@Skip, @SKIP_FALLBACK) ROWS
        FETCH NEXT ISNULL(@Take, @TAKE_FALLBACK) ROWS ONLY;
    END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Flashcard_IsInStack_tr]
	@StackId int,
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		IF EXISTS (SELECT * FROM Flashcard WHERE StackId = @StackId AND FlashcardId = @FlashcardId)
			SELECT 1 AS Result;
		ELSE
			SELECT 0 AS Result;
	END
RETURN;",
		@"CREATE PROCEDURE [dbo].[Flashcard_MoveStack_tr]
	@FlashcardId int,
	@StackId int
AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE Flashcard SET StackId = @StackId WHERE FlashcardId = @FlashcardId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Flashcard_Update_tr]
	@FlashcardId int,
	@Front nvarchar(255),
	@Back nvarchar(255)
AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE Flashcard SET Front = @Front, Back = @Back WHERE FlashcardId = @FlashcardId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[History_Count_tr]
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT
			COUNT(H.HistoryId) as HistorySessions
		FROM History AS H;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[History_Create_tr]
	@StartedAt datetime2(7),
	@StackId int
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[History] ([StartedAt], [StackId])
		VALUES (@StartedAt, @StackId);

		SELECT SCOPE_IDENTITY();
	END
RETURN SCOPE_IDENTITY();",
		@"CREATE PROCEDURE [dbo].[History_DeleteUnused_tr]
	@HistoryId int
AS
	BEGIN
		SET NOCOUNT ON;

		DELETE FROM History
		WHERE HistoryId = @HistoryId
			AND NOT EXISTS (SELECT 1 FROM StudyResult WHERE HistoryId = @HistoryId);
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[History_GetByStackAndDateOrCreate_tr]
	@StackId int,
	@StartedAt datetime2(7)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @HistoryId int;
		DECLARE @HistoryCount int;

		SELECT @HistoryCount = COUNT(1) FROM History WHERE StackId = @StackId AND StartedAt = @StartedAt;

		IF @HistoryCount = 0
			BEGIN
				INSERT INTO History (StackId, StartedAt) VALUES (@StackId, @StartedAt);
				SELECT @HistoryId = SCOPE_IDENTITY();
			END
		ELSE
			BEGIN
				SELECT @HistoryId = HistoryId FROM History WHERE StackId = @StackId AND StartedAt = @StartedAt;
			END

		SELECT @HistoryId AS HistoryId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[History_GetMultiple_tr]
    @Skip INT = NULL,
    @Take INT = NULL
AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

        SELECT
            H.HistoryId, H.StartedAt,
            S.ViewName,
            COUNT(R.FlashcardId) as CardsStudied,
            SUM(CASE WHEN R.WasCorrect = 1 THEN 1 ELSE 0 END) AS CorrectAnswers
        FROM History AS H
        INNER JOIN Stack AS S ON H.StackId = S.StackId
        LEFT JOIN StudyResult AS R ON H.HistoryId = R.HistoryId
        GROUP BY H.HistoryId, H.StartedAt, S.ViewName
        ORDER BY H.HistoryId
        OFFSET ISNULL(@Skip, @SKIP_FALLBACK) ROWS
        FETCH NEXT ISNULL(@Take, @TAKE_FALLBACK) ROWS ONLY;
    END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[History_GetMultipleByFlashcard_tr]
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT H.HistoryId, H.StackId, H.StartedAt FROM History H
		INNER JOIN StudyResult SR ON H.HistoryId = SR.HistoryId
		WHERE SR.FlashcardId = @FlashcardId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Stack_Count_tr]
    @Skip INT = NULL,
    @Take INT = NULL
AS
	BEGIN
		SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

		SELECT
			COUNT(S.StackId) as Stacks
		FROM Stack AS S;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Stack_Create_tr]
	@ViewName nvarchar(255),
	@SortName nvarchar(255)
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[Stack] ([ViewName], [SortName])
		VALUES (@ViewName, @SortName);
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Stack_Delete_tr]
	@StackId int
AS
	BEGIN
		SET NOCOUNT ON;

		DELETE FROM [dbo].[Stack]
		WHERE [StackId] = @StackId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Stack_GetByFlashcardId_tr]
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT TOP 1
			S.StackId, S.ViewName,
			(SELECT COUNT(1) FROM Flashcard AS FC WHERE FC.StackId = S.StackId) as Cards,
			MAX(H.StartedAt) as LastStudied
		FROM Stack AS S
		LEFT JOIN Flashcard AS F ON S.StackId = F.StackId
		LEFT JOIN History AS H ON S.StackId = H.StackId
		WHERE F.FlashcardId = @FlashcardId
		GROUP BY S.StackId, S.ViewName;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Stack_GetById_tr]
	@StackId int
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT TOP 1
			S.StackId, S.ViewName,
			COUNT(DISTINCT F.FlashcardId) as Cards,
			MAX(H.StartedAt) as LastStudied
		FROM Stack AS S
		LEFT JOIN Flashcard AS F ON S.StackId = F.StackId
		LEFT JOIN History AS H ON S.StackId = H.StackId
		WHERE S.StackId = @StackId
		GROUP BY S.StackId, S.ViewName;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Stack_GetBySortName_tr]
	@SortName nvarchar(255)
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT TOP 1
			S.StackId, S.ViewName,
			COUNT(DISTINCT F.FlashcardId) as Cards,
			MAX(H.StartedAt) as LastStudied
		FROM Stack AS S
		LEFT JOIN Flashcard AS F ON S.StackId = F.StackId
		LEFT JOIN History AS H ON S.StackId = H.StackId
		WHERE S.SortName = @SortName
		GROUP BY S.StackId, S.ViewName;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Stack_GetMultiple_tr]
    @Skip INT = NULL,
    @Take INT = NULL
AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

        SELECT
            S.StackId, S.ViewName,
            COUNT(DISTINCT F.FlashcardId) as Cards,
            MAX(H.StartedAt) as LastStudied
        FROM Stack AS S
        LEFT JOIN Flashcard AS F ON S.StackId = F.StackId
        LEFT JOIN History AS H ON S.StackId = H.StackId
        GROUP BY S.StackId, S.ViewName
        ORDER BY S.StackId
        OFFSET ISNULL(@Skip, @SKIP_FALLBACK) ROWS
        FETCH NEXT ISNULL(@Take, @TAKE_FALLBACK) ROWS ONLY;
    END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[Stack_Rename_tr]
	@StackId int,
	@ViewName nvarchar(255),
	@SortName nvarchar(255)
AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE [dbo].[Stack]
		SET [ViewName] = @ViewName, [SortName] = @SortName
		WHERE [StackId] = @StackId;
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[StudyResult_Create_tr]
	@HistoryId int,
	@FlashcardId int,
	@WasCorrect bit,
	@AnsweredAt datetime2(7)
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[StudyResult] ([HistoryId], [FlashcardId], [WasCorrect], [AnsweredAt])
		VALUES (@HistoryId, @FlashcardId, @WasCorrect, @AnsweredAt);
	END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[StudyResult_GetMultiple_tr]
    @HistoryId INT,
    @Skip INT = NULL,
    @Take INT = NULL
AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

        SELECT
            ROW_NUMBER() OVER (ORDER BY StudyResultId) AS Ordinal,
            F.Front,
            R.AnsweredAt, R.WasCorrect
        FROM StudyResult AS R
        INNER JOIN Flashcard AS F ON R.FlashcardId = F.FlashcardId
        WHERE R.HistoryId = @HistoryId
        ORDER BY R.StudyResultId
        OFFSET ISNULL(@Skip, @SKIP_FALLBACK) ROWS
        FETCH NEXT ISNULL(@Take, @TAKE_FALLBACK) ROWS ONLY;
    END
RETURN 0;",
		@"CREATE PROCEDURE [dbo].[StudyResult_MoveMultiple_tr]
	@FlashcardId int,
	@OldHistoryId int,
	@NewHistoryId int
AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE StudyResult SET HistoryId = @NewHistoryId WHERE FlashcardId = @FlashcardId AND HistoryId = @OldHistoryId;
	END
RETURN 0;" };

	private const string _addSampleData = @"--Data from https://en.wiktionary.org/wiki/Appendix:Swadesh_lists

SET IDENTITY_INSERT [dbo].[Stack] ON
INSERT INTO [dbo].[Stack] ([StackId], [SortName], [ViewName]) VALUES (1, N'bulgarian', N'Bulgarian')
INSERT INTO [dbo].[Stack] ([StackId], [SortName], [ViewName]) VALUES (2, N'finnish', N'Finnish')
INSERT INTO [dbo].[Stack] ([StackId], [SortName], [ViewName]) VALUES (3, N'german', N'German')
INSERT INTO [dbo].[Stack] ([StackId], [SortName], [ViewName]) VALUES (4, N'greek', N'Greek')
INSERT INTO [dbo].[Stack] ([StackId], [SortName], [ViewName]) VALUES (5, N'indonesian', N'Indonesian')
INSERT INTO [dbo].[Stack] ([StackId], [SortName], [ViewName]) VALUES (6, N'japanese', N'Japanese')
INSERT INTO [dbo].[Stack] ([StackId], [SortName], [ViewName]) VALUES (7, N'latin', N'Latin')
INSERT INTO [dbo].[Stack] ([StackId], [SortName], [ViewName]) VALUES (8, N'swedish', N'Swedish')
SET IDENTITY_INSERT [dbo].[Stack] OFF

SET IDENTITY_INSERT [dbo].[Flashcard] ON
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (1, 1, N'What''s the Bulgarian word for: sun?', N'slǎnce')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (2, 1, N'What''s the Bulgarian word for: moon?', N'luna')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (3, 1, N'What''s the Bulgarian word for: star?', N'zvezda')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (4, 1, N'What''s the Bulgarian word for: water?', N'voda')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (5, 1, N'What''s the Bulgarian word for: rain?', N'dǎžd')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (6, 1, N'What''s the Bulgarian word for: river?', N'reka')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (7, 1, N'What''s the Bulgarian word for: lake?', N'ezero')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (8, 1, N'What''s the Bulgarian word for: sea?', N'more')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (9, 1, N'What''s the Bulgarian word for: salt?', N'sol')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (10, 1, N'What''s the Bulgarian word for: stone?', N'kamǎk')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (11, 1, N'What''s the Bulgarian word for: sand?', N'pjasǎk')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (12, 1, N'What''s the Bulgarian word for: dust?', N'prah')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (13, 1, N'What''s the Bulgarian word for: earth?', N'zemja')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (14, 1, N'What''s the Bulgarian word for: cloud?', N'oblak')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (15, 1, N'What''s the Bulgarian word for: fog?', N'mǎgla')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (16, 1, N'What''s the Bulgarian word for: sky?', N'nebe')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (17, 1, N'What''s the Bulgarian word for: wind?', N'vjatǎr')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (18, 1, N'What''s the Bulgarian word for: snow?', N'snjag')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (19, 1, N'What''s the Bulgarian word for: ice?', N'led')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (20, 1, N'What''s the Bulgarian word for: smoke?', N'pušek')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (21, 1, N'What''s the Bulgarian word for: fire?', N'ogǎn')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (22, 1, N'What''s the Bulgarian word for: ash?', N'pepel')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (23, 2, N'What''s the Finnish word for: to drink?', N'juoda')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (24, 2, N'What''s the Finnish word for: to eat?', N'syödä')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (25, 2, N'What''s the Finnish word for: to bite?', N'purra')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (26, 2, N'What''s the Finnish word for: to suck?', N'imeä')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (27, 2, N'What''s the Finnish word for: to spit?', N'sylkeä')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (28, 2, N'What''s the Finnish word for: to vomit?', N'oksentaa')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (29, 2, N'What''s the Finnish word for: to blow?', N'puhaltaa')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (30, 2, N'What''s the Finnish word for: to breathe?', N'hengittää')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (31, 2, N'What''s the Finnish word for: to laugh?', N'nauraa')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (32, 2, N'What''s the Finnish word for: to see?', N'nähdä')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (33, 2, N'What''s the Finnish word for: to hear?', N'kuulla')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (34, 2, N'What''s the Finnish word for: to know?', N'tietää')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (35, 2, N'What''s the Finnish word for: to think?', N'ajatella')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (36, 2, N'What''s the Finnish word for: to smell?', N'haistaa')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (37, 2, N'What''s the Finnish word for: to fear?', N'pelätä')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (38, 2, N'What''s the Finnish word for: to sleep?', N'nukkua')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (39, 2, N'What''s the Finnish word for: to live?', N'elää')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (40, 2, N'What''s the Finnish word for: to die?', N'kuolla')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (41, 2, N'What''s the Finnish word for: to kill?', N'tappaa')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (42, 3, N'What''s the German word for: mother?', N'Mutter')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (43, 3, N'What''s the German word for: father?', N'Vater')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (44, 3, N'What''s the German word for: animal?', N'Tier')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (45, 3, N'What''s the German word for: fish?', N'Fisch')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (46, 3, N'What''s the German word for: bird?', N'Vogel')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (47, 3, N'What''s the German word for: dog?', N'Hund')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (48, 3, N'What''s the German word for: louse?', N'Laus')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (49, 3, N'What''s the German word for: snake?', N'Schlange')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (50, 3, N'What''s the German word for: worm?', N'Wurm')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (51, 3, N'What''s the German word for: tree?', N'Baum')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (52, 3, N'What''s the German word for: forest?', N'Forst')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (53, 3, N'What''s the German word for: stick?', N'Stock')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (54, 3, N'What''s the German word for: fruit?', N'Frucht')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (55, 3, N'What''s the German word for: seed?', N'Samen')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (56, 3, N'What''s the German word for: leaf?', N'Blatt')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (57, 3, N'What''s the German word for: root?', N'Wurzel')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (58, 4, N'What''s the Greek word for: day?', N'méra')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (59, 4, N'What''s the Greek word for: year?', N'chrónos')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (60, 4, N'What''s the Greek word for: warm?', N'zestós')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (61, 4, N'What''s the Greek word for: cold?', N'krýos')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (62, 4, N'What''s the Greek word for: full?', N'gemátos')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (63, 4, N'What''s the Greek word for: new?', N'kainoúrgios')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (64, 4, N'What''s the Greek word for: old?', N'paliós')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (65, 4, N'What''s the Greek word for: good?', N'kalós')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (66, 4, N'What''s the Greek word for: bad?', N'kakós')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (67, 4, N'What''s the Greek word for: rotten?', N'sápios')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (68, 4, N'What''s the Greek word for: dirty?', N'vrómikos')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (69, 4, N'What''s the Greek word for: straight?', N'ísios')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (70, 4, N'What''s the Greek word for: round?', N'strongylós')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (71, 5, N'What''s the Indonesian word for: one?', N'satu')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (72, 5, N'What''s the Indonesian word for: two?', N'dua')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (73, 5, N'What''s the Indonesian word for: three?', N'tiga')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (74, 5, N'What''s the Indonesian word for: four?', N'empat')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (75, 5, N'What''s the Indonesian word for: five?', N'lima')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (76, 5, N'What''s the Indonesian word for: big?', N'besar')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (77, 5, N'What''s the Indonesian word for: long?', N'panjang')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (78, 5, N'What''s the Indonesian word for: wide?', N'lebar')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (79, 5, N'What''s the Indonesian word for: thick?', N'tebal')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (80, 5, N'What''s the Indonesian word for: heavy?', N'berat')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (81, 5, N'What''s the Indonesian word for: small?', N'kecil')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (82, 5, N'What''s the Indonesian word for: short?', N'pendek')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (83, 5, N'What''s the Indonesian word for: narrow?', N'sempit')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (84, 5, N'What''s the Indonesian word for: thin?', N'tipis')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (85, 5, N'What''s the Indonesian word for: woman?', N'perempuan')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (86, 5, N'What''s the Indonesian word for: man (adult male)?', N'lelaki')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (87, 6, N'What''s the Japanese word for: I ?', N'わたし')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (88, 6, N'What''s the Japanese word for: you (singular)?', N'あなた')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (89, 6, N'What''s the Japanese word for: he ?', N'かれ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (90, 6, N'What''s the Japanese word for: we ?', N'わたしたち')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (91, 6, N'What''s the Japanese word for: you (plural)?', N'あなたたち')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (92, 6, N'What''s the Japanese word for: they ?', N'かれら')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (93, 6, N'What''s the Japanese word for: this ?', N'これ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (94, 6, N'What''s the Japanese word for: that ?', N'それ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (95, 6, N'What''s the Japanese word for: here ?', N'ここ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (96, 6, N'What''s the Japanese word for: there ?', N'そこ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (97, 6, N'What''s the Japanese word for: who ?', N'だれ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (98, 6, N'What''s the Japanese word for: what ?', N'なに')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (99, 6, N'What''s the Japanese word for: where ?', N'どこ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (100, 6, N'What''s the Japanese word for: when ?', N'いつ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (101, 6, N'What''s the Japanese word for: how ?', N'いかが')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (102, 6, N'What''s the Japanese word for: not ?', N'ない ')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (103, 7, N'What''s the Latin word for: breast?', N'mamma')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (104, 7, N'What''s the Latin word for: heart?', N'cor')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (105, 7, N'What''s the Latin word for: liver?', N'iecur')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (106, 7, N'What''s the Latin word for: to drink?', N'bibo')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (107, 7, N'What''s the Latin word for: to eat?', N'edo')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (108, 7, N'What''s the Latin word for: to bite?', N'mordeo')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (109, 7, N'What''s the Latin word for: to suck?', N'sugo')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (110, 7, N'What''s the Latin word for: to spit?', N'spuo')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (111, 7, N'What''s the Latin word for: to vomit?', N'vomo')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (112, 7, N'What''s the Latin word for: to blow?', N'inflo')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (113, 7, N'What''s the Latin word for: to breathe?', N'respiro')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (114, 7, N'What''s the Latin word for: to laugh?', N'rideo')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (115, 7, N'What''s the Latin word for: to see?', N'video')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (116, 7, N'What''s the Latin word for: to hear?', N'audio')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (117, 7, N'What''s the Latin word for: to know?', N'scio')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (118, 8, N'What''s the Swedish word for: egg?', N'ägg')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (119, 8, N'What''s the Swedish word for: horn?', N'horn')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (120, 8, N'What''s the Swedish word for: tail?', N'svans')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (121, 8, N'What''s the Swedish word for: feather?', N'fjäder')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (122, 8, N'What''s the Swedish word for: hair?', N'hår')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (123, 8, N'What''s the Swedish word for: head?', N'huvud')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (124, 8, N'What''s the Swedish word for: ear?', N'öra')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (125, 8, N'What''s the Swedish word for: eye?', N'öga')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (126, 8, N'What''s the Swedish word for: nose?', N'näsa')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (127, 8, N'What''s the Swedish word for: mouth?', N'mun')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (128, 8, N'What''s the Swedish word for: tooth?', N'tand')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (129, 8, N'What''s the Swedish word for: tongue (organ)?', N'tunga')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (130, 8, N'What''s the Swedish word for: fingernail?', N'nagel')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (131, 8, N'What''s the Swedish word for: foot?', N'fot')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (132, 8, N'What''s the Swedish word for: leg?', N'ben')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (133, 8, N'What''s the Swedish word for: knee?', N'knä')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (134, 8, N'What''s the Swedish word for: hand?', N'hand')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (135, 8, N'What''s the Swedish word for: wing?', N'vinge')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (136, 8, N'What''s the Swedish word for: belly?', N'mage')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (137, 8, N'What''s the Swedish word for: guts?', N'inälvor')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (138, 8, N'What''s the Swedish word for: neck?', N'hals')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (139, 8, N'What''s the Swedish word for: back?', N'rygg')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (140, 8, N'What''s the Swedish word for: breast?', N'bröst')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (141, 8, N'What''s the Swedish word for: heart?', N'hjärta')
INSERT INTO [dbo].[Flashcard] ([FlashcardId], [StackId], [Front], [Back]) VALUES (142, 8, N'What''s the Swedish word for: liver?', N'lever ')
SET IDENTITY_INSERT [dbo].[Flashcard] OFF";
}
