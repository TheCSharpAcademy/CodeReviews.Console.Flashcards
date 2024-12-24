using Microsoft.Data.SqlClient;
using System.Data;
using System.Configuration;
using Flashcards.Models;
using Dapper;

namespace Flashcards.Database;

class DatabaseManager
{
    internal void CreateDatabase()
    {
        string? connectionString = ConfigurationManager.AppSettings["masterConnectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'FlashcardsDB')
            BEGIN
                CREATE DATABASE FlashcardsDB
            END";
        command.ExecuteNonQuery();
    }
    internal void CreateStacksTable()
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'stacks')
            CREATE TABLE stacks (
            StackId INT IDENTITY(1,1) PRIMARY KEY,
            StackName NVARCHAR(255) NOT NULL UNIQUE 
            );";
        command.ExecuteNonQuery();
    }

    internal void CreateFlashcardsTable()
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'flashcards')
            CREATE TABLE flashcards (
            FlashcardId INT IDENTITY(1,1) PRIMARY KEY,
            Front NVARCHAR(255) NOT NULL,
            Back NVARCHAR(255) NOT NULL,
            StackId INT,
            FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE
            );";
        command.ExecuteNonQuery();
    }

    internal void CreateStudySessionsTable()
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'study_sessions')
            CREATE TABLE study_sessions (
            StudySessionId INT IDENTITY(1,1) PRIMARY KEY,
            Date DATE NOT NULL,
            Score INT NOT NULL,
            StackId INT,
            FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE
            );";
        command.ExecuteNonQuery();
    }

    internal List<Stack> GetAllStacksFromDB()
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        string? query = @"SELECT * from stacks ORDER BY StackId";

        var stacksDataFromDB = connection.Query<Stack>(query).ToList();
        return stacksDataFromDB;
    }

    internal List<FlashcardsModel> GetAllFlashcardsFromDB()
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        string? query = @"SELECT * from flashcards ORDER BY StackId";

        var flashcardsFromDB = connection.Query<FlashcardsModel>(query).ToList();

        return flashcardsFromDB;
    }

    internal void CreateStack(string? stack)
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO stacks (StackName) VALUES(@StackName) 
        ;";
        command.Parameters.Add($"@StackName", SqlDbType.NVarChar).Value = stack;
        command.ExecuteNonQuery();
    }

    internal void CreateFlashcard(string? front, string? back, int stackId)
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO flashcards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)
        ;";
        command.Parameters.Add($"@Front", SqlDbType.NVarChar).Value = front;
        command.Parameters.Add($"@Back", SqlDbType.NVarChar).Value = back;
        command.Parameters.Add($"@StackId", SqlDbType.Int).Value = stackId;
        command.ExecuteNonQuery();
    }

    internal int FindMatchingRecord(string? stackName)
    {
        int stackId;
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @$"SELECT StackId FROM stacks WHERE StackName = '{stackName}'";
        return stackId = (int)command.ExecuteScalar();
    }

    internal string? FindMatchingStackNameRecord(int id)
    {
        string? stackName;

        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @$"SELECT StackName FROM stacks WHERE StackId = '{id}'";
        return stackName = (string)command.ExecuteScalar();
    }

    internal int DeleteAStackFromDB(string? stackName)
    {
        int stackId = FindMatchingRecord(stackName);

        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"DELETE from stacks WHERE StackId = @id";
        command.Parameters.AddWithValue("@id", stackId);
        int rowDeleted = command.ExecuteNonQuery();

        return rowDeleted;
    }

    internal void UpdateAStackFromDB(string? stackName, string? updatedStackName)
    {
        int stackId = FindMatchingRecord(stackName);

        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"UPDATE stacks SET StackName=@updatedStackName WHERE StackId=@stackId";
        command.Parameters.AddWithValue("@updatedStackName", updatedStackName);
        command.Parameters.AddWithValue("@stackId", stackId);
        command.ExecuteNonQuery();
    }

    internal void UpdateAFlashcardFromDB(string? front, string? back, int flashcardId)
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"UPDATE flashcards SET";

        if (!string.IsNullOrEmpty(front))
        {
            // Add comma if updating 'Front' to separate fields, no comma for last field.
            command.CommandText += @" Front=@front,";
            command.Parameters.AddWithValue("@front", front);
        }

        if (!string.IsNullOrEmpty(back))
        {
            command.CommandText += @" Back=@back";
            command.Parameters.AddWithValue("@back", back);
        }
        command.CommandText = command.CommandText.TrimEnd(',');
        command.CommandText += @" WHERE FlashcardId=@flashcardId";
        command.Parameters.AddWithValue("@FlashcardId", flashcardId);
        command.ExecuteNonQuery();
    }

    internal int DeleteAFlashcardFromDB(int flashcardId)
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"DELETE from flashcards WHERE FlashcardId = @flashcardId";
        command.Parameters.AddWithValue("@FlashcardId", flashcardId);
        int rowDeleted = command.ExecuteNonQuery();

        return flashcardId;
    }

    internal List<FlashcardsModel> GetStackSpecificFlashcards(string? stackName)
    {
        var stackId = FindMatchingRecord(stackName);

        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        string query = @$"SELECT * FROM flashcards WHERE StackId = '{stackId}'";

        var flashcardsFromDB = connection.Query<FlashcardsModel>(query).ToList();

        return flashcardsFromDB;
    }

    internal void CreateAStudySession(DateTime date, int score, int stackId)
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO study_sessions (Date, Score, StackId) VALUES (@Date, @Score, @StackId)
        ;";
        command.Parameters.Add($"@Date", SqlDbType.Date).Value = date;
        command.Parameters.Add($"@Score", SqlDbType.Int).Value = score;
        command.Parameters.Add($"@StackId", SqlDbType.Int).Value = stackId;
        command.ExecuteNonQuery();
    }

    internal List<StudySession> GetStackSpecificStudySessionData(string? stackName)
    {
        var stackId = FindMatchingRecord(stackName);

        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        string query = @$"SELECT * FROM study_sessions WHERE StackId = '{stackId}'";

        var studySessionsFromDB = connection.Query<StudySession>(query).ToList();

        return studySessionsFromDB;
    }

    internal List<StudySessionSummary> GetStudySessionSummaryData(int year)
    {
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        string query = @$"
            SELECT StackName,
                isNULL([January], 0) AS January,
                isNULL([February], 0) AS February,
                isNULL([March], 0) AS March,
                isNULL([April], 0) AS April,
                isNULL([May], 0) AS May,
                isNULL([June], 0) AS June,
                isNULL([July], 0) AS July,
                isNULL([August], 0) AS August,
                isNULL([September], 0) AS September,
                isNULL([October], 0) AS October,
                isNULL([November], 0) AS November,
                isNULL([December], 0) AS December 
            FROM (
                SELECT stacks.StackName, DATENAME(MONTH, study_sessions.Date) AS Month, study_sessions.Score 
                FROM study_sessions
                INNER JOIN stacks ON study_sessions.StackId = stacks.StackId
                WHERE YEAR(study_sessions.Date) = @Year
            ) SourceTable
            PIVOT(
                SUM(Score)
                FOR Month IN([January], [February], [March], [April], [May], [June],
                           [July], [August], [September], [October], [November], [December])
            ) PivotTable";

        var studySessionSummary = connection.Query<StudySessionSummary>(query, new { Year = year }).ToList();
        return studySessionSummary;
    }
}


