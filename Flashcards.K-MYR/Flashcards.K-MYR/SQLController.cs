using Flashcards.K_MYR.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Flashcards.K_MYR;


internal static class SqlController
{
    static private readonly string ServerName = ConfigurationManager.AppSettings.Get("sqlServerName");

    static private readonly string DBName = ConfigurationManager.AppSettings.Get("dbName");

    static private readonly string ConnectionString = @$"Data Source= {ServerName}; Initial Catalog = {DBName}";

    internal static void CreateDbIfNotExists()
    {
        using SqlConnection connection = new(@$"Data Source={ServerName}");
        connection.Open();
        SqlCommand cmd = connection.CreateCommand();

        cmd.CommandText = $@"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{DBName}')
                            CREATE DATABASE {DBName};";
        cmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static void CreateTablesIfNotExists()
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand cmd = connection.CreateCommand();

        cmd.CommandText = @"IF OBJECT_ID(N'dbo.Stacks', N'U') IS NULL
                            CREATE TABLE dbo.Stacks (
                                                     StackId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
                                                     Name NVARCHAR(25) NOT NULL UNIQUE,
                                                     NumberOfCards INT DEFAULT 0,
                                                     Created DATETIME NOT NULL DEFAULT GETDATE()
                                                    );    

                             IF OBJECT_ID(N'dbo.Flashcards', N'U') IS NULL 
                             BEGIN
                             CREATE TABLE dbo.Flashcards (
                                                          FlashcardId INT IDENTITY(1,1)PRIMARY KEY NOT NULL,
                                                          StackId INT NOT NULL,
                                                          FrontText VARCHAR(25) NOT NULL,
                                                          BackText VARCHAR(25) NOT NULL,
                                                          Created DATETIME NOT NULL DEFAULT GETDATE()
                                                         ); 
                             ALTER TABLE dbo.Flashcards WITH Check ADD CONSTRAINT FK_Stacks_Flashcards 
                             FOREIGN KEY (StackId) REFERENCES Stacks(StackId)                             
                             END      

                             IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
                             BEGIN
                             CREATE TABLE dbo.Sessions (
                                                        SessionId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
                                                        StackId INT NOT NULL,
                                                        Date DATETIME NOT NULL DEFAULT GETDATE(),
                                                        Score INT NOT NULL,
                                                        DurationInTicks BIGINT NOT NULL,
                                                       );                           
                             ALTER TABLE dbo.Sessions WITH Check ADD CONSTRAINT FK_Stacks_Sessions 
                             FOREIGN KEY (StackId) REFERENCES Stacks(StackId);
                             END";

        cmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static List<CardStack> SelectStacksFromDB(string columns = "*", string args = "")
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();
        command.CommandText = $@"SELECT {columns} FROM dbo.Stacks {args} ORDER BY Name";

        SqlDataReader reader = command.ExecuteReader();

        List<CardStack> stacks = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                stacks.Add(new CardStack
                {
                    StackId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    NumberOfCards = reader.GetInt32(2),
                    Created = reader.GetDateTime(3),
                });
            }
        }
        return stacks;
    }

    internal static int StackNameExists(string name)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"SELECT 1 FROM dbo.Stacks WHERE Name = '{name}'";

        return Convert.ToInt32(command.ExecuteScalar());
    }

    internal static void DeleteStack(int iD)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"DELETE FROM dbo.Stacks WHERE StackId = {iD}";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static void InsertStack(string name)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"INSERT INTO dbo.Stacks (Name) VALUES ('{name}')";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static void UpdateStack(string args, int iD)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"UPDATE dbo.Stacks SET {args} WHERE StackId = {iD}";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static List<Flashcard> SelectFlashcardsFromDB(string columns = "*", string args = "")
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();
        command.CommandText = $@"SELECT {columns} FROM dbo.Flashcards {args} ORDER BY Created";

        SqlDataReader reader = command.ExecuteReader();

        List<Flashcard> flashCards = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                flashCards.Add(new Flashcard
                {
                    FlashcardId = reader.GetInt32(0),
                    StackId = reader.GetInt32(1),
                    FrontText = reader.GetString(2),
                    BackText = reader.GetString(3),
                    Created = reader.GetDateTime(4),
                });
            }
        }
        return flashCards;
    }

    internal static void DeleteFlashcard(int iD)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"DELETE FROM dbo.Flashcards WHERE FlashcardId = {iD}";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static void DeleteFlashcardsByStackID(int iD)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"DELETE FROM dbo.Flashcards WHERE StackId = {iD}";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static void InsertFlashcard(int stackID, string frontText, string backText)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"INSERT INTO dbo.Flashcards(StackId, FrontText, BackText) VALUES ({stackID}, '{frontText}', '{backText}')";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static void UpdateFlashcard(string args, int iD)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"UPDATE dbo.Flashcards SET {args} WHERE FlashcardId = {iD}";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static void InsertSession(int stackID, long durationInTicks, int score)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"INSERT INTO dbo.Sessions (StackId, DurationInTicks, Score) VALUES ({stackID}, {durationInTicks}, {score})";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static void DeleteSessionsByStackID(int iD)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"DELETE FROM dbo.Sessions WHERE StackId = {iD}";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static List<Session> SelectSessionsFromDB()
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();
        command.CommandText = $@"SELECT dbo.Sessions.SessionId, dbo.Stacks.StackId, dbo.Stacks.Name, dbo.Sessions.Date, dbo.Sessions.Score, dbo.Sessions.DurationInTicks FROM dbo.Sessions JOIN dbo.Stacks ON dbo.Sessions.StackId = dbo.Stacks.StackId";

        SqlDataReader reader = command.ExecuteReader();

        List<Session> sessions = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                sessions.Add(new Session
                {
                    SessionId = reader.GetInt32(0),
                    StackId = reader.GetInt32(1),
                    StackName = reader.GetString(2),
                    Date = reader.GetDateTime(3),
                    Score = reader.GetInt32(4),
                    Duration = TimeSpan.FromTicks(reader.GetInt64(5)),

                });
            }
        }
        return sessions;
    }

    internal static List<ReportEntryScore> SumScorePerMonth(string year)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $@"SELECT  Name,
                                        [1] AS Jan,
                                        [2] AS Feb,
                                        [3] AS Mrz,
                                        [4] AS Apr,
                                        [5] AS Mai,
                                        [6] AS Jun,
                                        [7] AS Jul,
                                        [8] AS Aug,
                                        [9] AS Sep,
                                        [10] AS Okt,
                                        [11] AS Nov,
                                        [12] AS Dez
                                FROM
                                    (SELECT
                                        dbo.Stacks.Name,
                                        dbo.Sessions.Score,
                                        MONTH(dbo.Sessions.Date) as TMonth
                                    FROM
                                        dbo.Sessions                                    
                                    JOIN 
                                        dbo.Stacks ON dbo.Sessions.StackId = dbo.Stacks.StackId
                                    WHERE
                                        dbo.Sessions.Date BETWEEN '{year}-01-01' AND '{year}-12-31'
                                    )
                                SOURCE
                                PIVOT
                                (
                                    SUM(Score)
                                    FOR TMonth
                                    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )
                                ) AS pvtMonth";

        var reader = command.ExecuteReader();

        List<ReportEntryScore> tableData = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(new ReportEntryScore
                {
                    StackName = reader.GetString(0),
                    JanCount = SafeGetInt32(reader, 1),
                    FebCount = SafeGetInt32(reader, 2),
                    MarCount = SafeGetInt32(reader, 3),
                    AprilCount = SafeGetInt32(reader, 4),
                    MayCount = SafeGetInt32(reader, 5),
                    JuneCount = SafeGetInt32(reader, 6),
                    JulyCount = SafeGetInt32(reader, 7),
                    AugCount = SafeGetInt32(reader, 8),
                    SepCount = SafeGetInt32(reader, 9),
                    OctCount = SafeGetInt32(reader, 10),
                    NovCount = SafeGetInt32(reader, 11),
                    DecCount = SafeGetInt32(reader, 12),
                });
            }
        }
        return tableData;
    }

    internal static List<ReportEntryScore> AvgScorePerMonth(string year)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $@"SELECT  Name,
                                        [1] AS Jan,
                                        [2] AS Feb,
                                        [3] AS Mrz,
                                        [4] AS Apr,
                                        [5] AS Mai,
                                        [6] AS Jun,
                                        [7] AS Jul,
                                        [8] AS Aug,
                                        [9] AS Sep,
                                        [10] AS Okt,
                                        [11] AS Nov,
                                        [12] AS Dez
                                FROM
                                    (SELECT
                                        dbo.Stacks.Name,
                                        Cast(dbo.Sessions.Score as float) AS Score,
                                        MONTH(dbo.Sessions.Date) as TMonth
                                    FROM
                                        dbo.Sessions                                    
                                    JOIN 
                                        dbo.Stacks ON dbo.Sessions.StackId = dbo.Stacks.StackId
                                    WHERE
                                        dbo.Sessions.Date BETWEEN '{year}-01-01' AND '{year}-12-31'
                                    )
                                SOURCE
                                PIVOT
                                (
                                    AVG(Score)
                                    FOR TMonth
                                    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )
                                ) AS pvtMonth";

        var reader = command.ExecuteReader();

        List<ReportEntryScore> tableData = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(new ReportEntryScore
                {
                    StackName = reader.GetString(0),
                    JanCount = SafeGetFloat(reader, 1),
                    FebCount = SafeGetFloat(reader, 2),
                    MarCount = SafeGetFloat(reader, 3),
                    AprilCount = SafeGetFloat(reader, 4),
                    MayCount = SafeGetFloat(reader, 5),
                    JuneCount = SafeGetFloat(reader, 6),
                    JulyCount = SafeGetFloat(reader, 7),
                    AugCount = SafeGetFloat(reader, 8),
                    SepCount = SafeGetFloat(reader, 9),
                    OctCount = SafeGetFloat(reader, 10),
                    NovCount = SafeGetFloat(reader, 11),
                    DecCount = SafeGetFloat(reader, 12),
                });
            }
        }
        return tableData;
    }

    internal static List<ReportEntryTime> SumTimePerMonth(string year)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $@"SELECT  Name,
                                        [1] AS Jan,
                                        [2] AS Feb,
                                        [3] AS Mrz,
                                        [4] AS Apr,
                                        [5] AS Mai,
                                        [6] AS Jun,
                                        [7] AS Jul,
                                        [8] AS Aug,
                                        [9] AS Sep,
                                        [10] AS Okt,
                                        [11] AS Nov,
                                        [12] AS Dez
                                FROM
                                    (SELECT
                                        dbo.Stacks.Name,
                                        dbo.Sessions.DurationInTicks,
                                        MONTH(dbo.Sessions.Date) as TMonth
                                    FROM
                                        dbo.Sessions                                    
                                    JOIN 
                                        dbo.Stacks ON dbo.Sessions.StackId = dbo.Stacks.StackId
                                    WHERE
                                        dbo.Sessions.Date BETWEEN '{year}-01-01' AND '{year}-12-31'
                                    )
                                SOURCE
                                PIVOT
                                (
                                    SUM(DurationInTicks)
                                    FOR TMonth
                                    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )
                                ) AS pvtMonth";

        var reader = command.ExecuteReader();

        List<ReportEntryTime> tableData = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(new ReportEntryTime
                {
                    StackName = reader.GetString(0),
                    JanCount = TimeSpan.FromTicks(SafeGetInt64(reader, 1)).ToString("hh\\:mm\\:ss"),
                    FebCount = TimeSpan.FromTicks(SafeGetInt64(reader, 2)).ToString("hh\\:mm\\:ss"),
                    MarCount = TimeSpan.FromTicks(SafeGetInt64(reader, 3)).ToString("hh\\:mm\\:ss"),
                    AprilCount = TimeSpan.FromTicks(SafeGetInt64(reader, 4)).ToString("hh\\:mm\\:ss"),
                    MayCount = TimeSpan.FromTicks(SafeGetInt64(reader, 5)).ToString("hh\\:mm\\:ss"),
                    JuneCount = TimeSpan.FromTicks(SafeGetInt64(reader, 6)).ToString("hh\\:mm\\:ss"),
                    JulyCount = TimeSpan.FromTicks(SafeGetInt64(reader, 7)).ToString("hh\\:mm\\:ss"),
                    AugCount = TimeSpan.FromTicks(SafeGetInt64(reader, 8)).ToString("hh\\:mm\\:ss"),
                    SepCount = TimeSpan.FromTicks(SafeGetInt64(reader, 9)).ToString("hh\\:mm\\:ss"),
                    OctCount = TimeSpan.FromTicks(SafeGetInt64(reader, 10)).ToString("hh\\:mm\\:ss"),
                    NovCount = TimeSpan.FromTicks(SafeGetInt64(reader, 11)).ToString("hh\\:mm\\:ss"),
                    DecCount = TimeSpan.FromTicks(SafeGetInt64(reader, 12)).ToString("hh\\:mm\\:ss"),
                });
            }
        }

        return tableData;
    }

    internal static List<ReportEntryTime> AvgTimePerMonth(string year)
    {
        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $@"SELECT  Name,
                                        [1] AS Jan,
                                        [2] AS Feb,
                                        [3] AS Mrz,
                                        [4] AS Apr,
                                        [5] AS Mai,
                                        [6] AS Jun,
                                        [7] AS Jul,
                                        [8] AS Aug,
                                        [9] AS Sep,
                                        [10] AS Okt,
                                        [11] AS Nov,
                                        [12] AS Dez
                                FROM
                                    (SELECT
                                        dbo.Stacks.Name,
                                        dbo.Sessions.DurationInTicks,
                                        MONTH(dbo.Sessions.Date) as TMonth
                                    FROM
                                        dbo.Sessions                                    
                                    JOIN 
                                        dbo.Stacks ON dbo.Sessions.StackId = dbo.Stacks.StackId
                                    WHERE
                                        dbo.Sessions.Date BETWEEN '{year}-01-01' AND '{year}-12-31'
                                    )
                                SOURCE
                                PIVOT
                                (
                                    AVG(DurationInTicks)
                                    FOR TMonth
                                    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )
                                ) AS pvtMonth";

        var reader = command.ExecuteReader();

        List<ReportEntryTime> tableData = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(new ReportEntryTime
                {
                    StackName = reader.GetString(0),
                    JanCount = TimeSpan.FromTicks(SafeGetInt64(reader, 1)).ToString("hh\\:mm\\:ss"),
                    FebCount = TimeSpan.FromTicks(SafeGetInt64(reader, 2)).ToString("hh\\:mm\\:ss"),
                    MarCount = TimeSpan.FromTicks(SafeGetInt64(reader, 3)).ToString("hh\\:mm\\:ss"),
                    AprilCount = TimeSpan.FromTicks(SafeGetInt64(reader, 4)).ToString("hh\\:mm\\:ss"),
                    MayCount = TimeSpan.FromTicks(SafeGetInt64(reader, 5)).ToString("hh\\:mm\\:ss"),
                    JuneCount = TimeSpan.FromTicks(SafeGetInt64(reader, 6)).ToString("hh\\:mm\\:ss"),
                    JulyCount = TimeSpan.FromTicks(SafeGetInt64(reader, 7)).ToString("hh\\:mm\\:ss"),
                    AugCount = TimeSpan.FromTicks(SafeGetInt64(reader, 8)).ToString("hh\\:mm\\:ss"),
                    SepCount = TimeSpan.FromTicks(SafeGetInt64(reader, 9)).ToString("hh\\:mm\\:ss"),
                    OctCount = TimeSpan.FromTicks(SafeGetInt64(reader, 10)).ToString("hh\\:mm\\:ss"),
                    NovCount = TimeSpan.FromTicks(SafeGetInt64(reader, 11)).ToString("hh\\:mm\\:ss"),
                    DecCount = TimeSpan.FromTicks(SafeGetInt64(reader, 12)).ToString("hh\\:mm\\:ss"),
                });
            }
        }
        return tableData;
    }

    internal static float SafeGetFloat(this SqlDataReader reader, int ordinal, int defaultValue = 0)
    {
        if (!reader.IsDBNull(ordinal))
        {
            return (float)Math.Round(reader.GetDouble(ordinal), 2);
        }
        else
        {
            return defaultValue;
        }
    }

    internal static long SafeGetInt64(this SqlDataReader reader, int ordinal, int defaultValue = 0)
    {
        if (!reader.IsDBNull(ordinal))
        {
            return reader.GetInt64(ordinal);
        }
        else
        {
            return defaultValue;
        }
    }

    internal static long SafeGetInt32(this SqlDataReader reader, int ordinal, int defaultValue = 0)
    {
        if (!reader.IsDBNull(ordinal))
        {
            return reader.GetInt32(ordinal);
        }
        else
        {
            return defaultValue;
        }
    }
}
