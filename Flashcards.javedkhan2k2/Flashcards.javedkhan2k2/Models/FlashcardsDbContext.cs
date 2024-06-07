using Dapper;
using Microsoft.Data.SqlClient;

namespace Flashcards.Models;

internal class FlashcardsDbContext
{
    internal string ConnectionString { get; init; }

    public FlashcardsDbContext(string connectionString)
    {
        ConnectionString = connectionString;
        InitDatabase();
    }

    private SqlConnection? GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }

    private void InitDatabase()
    {
        CreateDatabase();
        CreateTables();
    }

    private void CreateDatabase()
    {
        var sql = @$"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Flashcards')
                    BEGIN
                        CREATE DATABASE Flashcards1;
                    END;
                    ";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql);
        }
    }

    private void CreateTables()
    {
        var sql = @$"
                IF OBJECT_ID(N'[dbo].[Stacks]', 'U') IS NULL
                CREATE TABLE Stacks(
                    Id INT IDENTITY (1,1) NOT NULL,
                    StackName Varchar(20) NOT NULL UNIQUE,
                    CONSTRAINT [PK_Stacks] Primary Key CLUSTERED ([Id] Asc)
                );
                IF OBJECT_ID(N'[dbo].[Flashcards]', 'U') IS NULL
                CREATE TABLE Flashcards(
                    Id INT IDENTITY (1,1) NOT NULL,
                    StackId INT NOT NULL,
                    Front VARCHAR(50) NOT NULL UNIQUE,
                    Back VARCHAR(50) NOT NULL,
                    CONSTRAINT [PK_FLASHCARDS] PRIMARY KEY CLUSTERED ([ID] ASC),
                    CONSTRAINT [FK1_Flashcards_Stacks] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stacks] ([Id]) ON DELETE CASCADE
                );
                IF OBJECT_ID(N'[dbo].[StudySessions]', 'U') IS NULL
                CREATE TABLE StudySessions(
                    Id INT IDENTITY (1,1) NOT NULL,
                    StackId INT NOT NULL,
                    TotalQuestions INT NOT NULL,
                    Score INT NOT NULL,
                    StudyDate DATE NOT NULL,
                    CONSTRAINT [PK_StudySessions] PRIMARY KEY CLUSTERED ([ID] ASC),
                    CONSTRAINT [FK_StudySessions_Stacks] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stacks] ([Id]) ON DELETE CASCADE
                );
            ";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql);
        }
    }

    private void SeedData()
    {
        var random = new Random();

        for (int i = 1; i < 11; i++)
        {
            AddStack(new StackDto { StackName = $"Stack{i}" });
            for (int j = 1; j < 21; j++)
            {
                FlashcardDto card = new FlashcardDto(i, $"Front{i}{j}", $"Back{i}{j}");
                AddFlashcard(card);
            }
        }

        DateTime startDate = new DateTime(2015, 1, 1);
        int range = (DateTime.Today - startDate).Days;
        for (int i = 0; i < 1000; i++)
        {
            StudySession session = new StudySession();
            session.StudyDate = startDate.AddDays(random.Next(range));
            session.StackId = random.Next(1, 11);
            session.Score = random.Next(0, 21);
            session.TotalQuestions = 20;
            AddStudySession(session);
        }
    }


    #region Stack DB Operations

    internal int AddStack(StackDto parameters)
    {
        var sql = @$"INSERT INTO Stacks (StackName) VALUES(@StackName);";
        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Execute(sql, parameters);
        }
    }

    internal int UpdateStackById(Stack parameters)
    {
        var sql = @$"UPDATE Stacks 
                        SET StackName = @StackName 
                        WHERE Id = @Id";
        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Execute(sql, parameters);
        }
    }

    internal void UpdateStackByName(string stackName, StackUpdateDto parameters)
    {
        var sql = @$"UPDATE Stacks
                            SET StackName = @StackName
                            WHERE StackName = @OldStackName";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql, new { OldStackName = stackName, StackName = parameters.StackName });
        }
    }

    internal void DeleteStackById(int id)
    {
        var sql = @$"DELETE Stacks WHERE Id = @Id";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql, new { Id = id });
        }
    }

    internal int DeleteStackByStackName(string stackName)
    {
        var sql = @$"DELETE Stacks WHERE StackName = @StackName";
        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Execute(sql, new { StackName = stackName });
        }
    }

    internal Stack? GetStackById(int id)
    {
        var sql = @$"SELECT * FROM Stacks WHERE Id = @Id";
        using (var connection = GetConnection())
        {
            connection.Open();
            Stack? stack = connection.QueryFirstOrDefault<Stack>(sql, new { Id = id });
            return stack;
        }
    }

    internal Stack? GetStackByName(string stackName)
    {
        var sql = @$"SELECT * FROM Stacks WHERE StackName = @StackName";
        using (var connection = GetConnection())
        {
            connection.Open();
            Stack? stack = connection.QueryFirstOrDefault<Stack>(sql, new { StackName = stackName });
            return stack;
        }
    }

    internal IEnumerable<Stack>? GetAllStacks()
    {
        var sql = @$"SELECT * FROM Stacks ORDER BY StackName";
        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Query<Stack>(sql);
        }
    }

    #endregion // End of Stack SQL Operations

    #region FlashCards DB Operations

    internal int AddFlashcard(FlashcardDto parameters)
    {
        var sql = @$"INSERT INTO Flashcards (StackId, Front, Back)
                            VALUES(@StackId, @Front, @Back)";
        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Execute(sql, parameters);
        }
    }

    internal int UpdateFlashcard(Flashcard parameters)
    {
        var sql = @$"UPDATE Flashcards 
                        SET
                            StackId = @StackId, 
                            Front = @Front , 
                            Back = @Back
                        WHERE Id = @Id";
        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Execute(sql, parameters);
        }
    }

    internal void DeleteFlashcard(int id)
    {
        var sql = @$"DELETE FROM Flashcards WHERE Id = @Id;";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql, new { Id = id });
        }
    }

    internal int DeleteFlashcardByFront(string front, int stackId)
    {
        var sql = @$"DELETE FROM Flashcards WHERE StackId = @StackId AND Front = @Front;";
        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Execute(sql, new { StackId = stackId, Front = front });
        }
    }

    internal Flashcard? GetFlashcardById(int id)
    {
        var sql = @$"SELECT * FROM Flashcards WHERE Id = @Id";
        using (var connection = GetConnection())
        {
            connection.Open();
            Flashcard? flashcard = connection.QueryFirstOrDefault<Flashcard>(sql, new { Id = id });
            return flashcard;
        }
    }

    internal IEnumerable<Flashcard>? GetAllFlashcards()
    {
        var sql = @$"SELECT * FROM Flashcards ORDER BY StackId";
        using (var connection = GetConnection())
        {
            connection.Open();
            IEnumerable<Flashcard>? flashcards = connection.Query<Flashcard>(sql);
            return flashcards;
        }
    }

    internal Flashcard? GetFlashCardByStackIdAndFront(int stackId, string front)
    {
        var sql = @$"SELECT * FROM Flashcards WHERE StackId = @StackId AND Front = @Front";
        using (var connection = GetConnection())
        {
            connection.Open();
            Flashcard? flashcard = connection.QueryFirstOrDefault<Flashcard>(sql, new { StackId = stackId, Front = front });
            return flashcard;
        }
    }
    internal IEnumerable<Flashcard>? GetAllFlashcardByStackId(int stackId)
    {
        var sql = @$"SELECT * FROM Flashcards WHERE StackId = @StackId";
        using (var connection = GetConnection())
        {
            connection.Open();
            IEnumerable<Flashcard>? flashcards = connection.Query<Flashcard>(sql, new { StackId = stackId });
            return flashcards;
        }
    }

    internal IEnumerable<FlashcardDto>? GetAllFlashcardDtoByStackId(int stackId)
    {
        var sql = @$"SELECT StackId, Front, Back FROM Flashcards WHERE StackId = @StackId";
        using (var connection = GetConnection())
        {
            connection.Open();
            IEnumerable<FlashcardDto>? flashcards = connection.Query<FlashcardDto>(sql, new { StackId = stackId });
            int i = 1;
            foreach (var flashcard in flashcards)
            {
                flashcard.Key = i++;
            }
            return flashcards;
        }
    }

    #endregion

    #region Study Sessions DB Operations

    internal void AddStudySession(StudySession parameters)
    {
        var sql = @$"INSERT INTO StudySessions (StackId, Score, TotalQuestions, StudyDate)
                            VALUES(@StackId, @Score, @TotalQuestions, @StudyDate);
                        ";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql, parameters);
        }
    }

    internal void UpdateStudySession(StudySession parameters)
    {
        var sql = @$"UPDATE StudySessions 
                        SET
                            StackId = @StackId, 
                            Score = @Score , 
                            TotalQuestions = @TotalQuestions,
                            StudyDate = @StudyDate
                        WHERE Id = @Id
                        ;";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql, parameters);
        }
    }

    internal void DeleteStudySession(int id)
    {
        var sql = @$"DELETE FROM StudySessions WHERE Id = @Id;";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql, new { Id = id });
        }
    }

    internal StudySession? GetByStudySessionId(int id)
    {
        var sql = @$"SELECT * FROM StudySessions WHERE Id = @Id";
        using (var connection = GetConnection())
        {
            connection.Open();
            StudySession? studySession = connection.QueryFirstOrDefault<StudySession>(sql, new { Id = id });
            return studySession;
        }
    }

    internal IEnumerable<StudySession>? GetAllStudySessionsByStackId(int stackId)
    {
        var sql = @$"SELECT * FROM StudySessions where StackId = @StackId ORDER BY StudyDate";
        using (var connection = GetConnection())
        {
            connection.Open();
            IEnumerable<StudySession>? studySessions = connection.Query<StudySession>(sql, new { StackId = stackId });
            return studySessions;
        }
    }

    #endregion

    #region Reports

    internal StackYearlyReport? GetStackReportByYear(int stackId, int year)
    {
        var sql = @$"
            Select 
                COALESCE([January], 0) AS [January], 
                COALESCE([February], 0) AS [February], 
                COALESCE([March], 0) AS [March],
                COALESCE([April], 0) AS [April], 
                COALESCE([May], 0) AS [May], 
                COALESCE([June], 0) AS [June], 
                COALESCE([July], 0) AS [July], 
                COALESCE([August], 0) AS [August], 
                COALESCE([September], 0) AS [September], 
                COALESCE([October], 0) AS [October], 
                COALESCE([November], 0) AS [November], 
                COALESCE([December], 0) AS [December]
            FROM (
                SELECT 
                    ss.Score as TScore,
                    DATENAME(MONTH, ss.StudyDate) AS [Month]
                FROM dbo.StudySessions AS ss
                WHERE 
                    YEAR(ss.StudyDate) = @Year and ss.StackId = @StackId
            ) AS SRC
            PIVOT (
                AVG(TScore)
                FOR [Month] IN ([January], [February], [March], [April], [May], [June], 
                                [July], [August], [September], [October], [November], [December])
            ) AS PivotTable
        ";
        using (var connection = GetConnection())
        {
            connection.Open();
            StackYearlyReport? report = connection.QueryFirstOrDefault<StackYearlyReport>(sql, new { Year = year, StackId = stackId });
            return report;
        }
    }

    internal IEnumerable<AllStackYearlyReport>? GetAllStacksReportByYear(int year)
    {
        var sql = @$"
                    Select 
                        Stack AS StackName,
                        COALESCE([January], 0) AS [January], 
                        COALESCE([February], 0) AS [February], 
                        COALESCE([March], 0) AS [March],
                        COALESCE([April], 0) AS [April], 
                        COALESCE([May], 0) AS [May], 
                        COALESCE([June], 0) AS [June], 
                        COALESCE([July], 0) AS [July], 
                        COALESCE([August], 0) AS [August], 
                        COALESCE([September], 0) AS [September], 
                        COALESCE([October], 0) AS [October], 
                        COALESCE([November], 0) AS [November], 
                        COALESCE([December], 0) AS [December]
                    FROM
                    (
                        SELECT 
                            ss.Score as TScore,
                            DATENAME(MONTH, ss.StudyDate) AS [Month],
                            st.StackName as Stack
                        FROM dbo.StudySessions AS ss JOIN Stacks as st on ss.StackId = st.Id 
                        WHERE 
                            YEAR(ss.StudyDate) = @Year
                    ) AS SRC
                    PIVOT
                    (
                        AVG(TScore)
                        FOR [MONTH] in ([January], [February], [March], [April], [May], [June], 
                                            [July], [August], [September], [October], [November], [December])
                    ) AS PVT
                ";
        using (var connection = GetConnection())
        {
            connection.Open();
            IEnumerable<AllStackYearlyReport>? report = connection.Query<AllStackYearlyReport>(sql, new { Year = year });
            return report;
        }
    }

    internal IEnumerable<AllStackYearlyReport>? GetAllStacksSessionsReportByYear(int year)
    {
        var sql = @$"
                    Select 
                        Stack AS StackName,
                        COALESCE([January], 0) AS [January], 
                        COALESCE([February], 0) AS [February], 
                        COALESCE([March], 0) AS [March],
                        COALESCE([April], 0) AS [April], 
                        COALESCE([May], 0) AS [May], 
                        COALESCE([June], 0) AS [June], 
                        COALESCE([July], 0) AS [July], 
                        COALESCE([August], 0) AS [August], 
                        COALESCE([September], 0) AS [September], 
                        COALESCE([October], 0) AS [October], 
                        COALESCE([November], 0) AS [November], 
                        COALESCE([December], 0) AS [December]
                    FROM
                    (
                        SELECT 
                            ss.StackId as CNT,
                            DATENAME(MONTH, ss.StudyDate) AS [Month],
                            st.StackName as Stack
                        FROM dbo.StudySessions AS ss JOIN Stacks as st on ss.StackId = st.Id 
                        WHERE 
                            YEAR(ss.StudyDate) = @Year
                    ) AS SRC
                    PIVOT
                    (
                        SUM(CNT)
                        FOR [MONTH] in ([January], [February], [March], [April], [May], [June], 
                                            [July], [August], [September], [October], [November], [December])
                    ) AS PVT
                ";
        using (var connection = GetConnection())
        {
            connection.Open();
            IEnumerable<AllStackYearlyReport>? report = connection.Query<AllStackYearlyReport>(sql, new { Year = year });
            return report;
        }
    }

    #endregion

}