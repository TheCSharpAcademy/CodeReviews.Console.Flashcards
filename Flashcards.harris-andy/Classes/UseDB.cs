using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.harris_andy.Classes;

namespace Flashcards.harris_andy;

public class UseDB
{
    public void InitializeDatabase()
    {
        string connectionStringMaster = "Server=localhost,1433;Database=master;User Id=SA;Password=SuperStrongSexyPassword123;TrustServerCertificate=True;";
        using var connectionMaster = new SqlConnection(connectionStringMaster);
        var checkDbQuery = @"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FlashCards')
                    BEGIN
                    CREATE DATABASE FlashCards;
                    END";
        connectionMaster.Execute(checkDbQuery);

        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var createTables = File.ReadAllText("./SQL_Queries/InitDB.sql");
        connection.Execute(createTables);
    }

    public void AddFlashCard(FlashCard flashCard, int stackID)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var parameters = new { Front = flashCard.Front, Back = flashCard.Back, StackID = stackID };
        string sql = "INSERT INTO flashcards (front, back, stackID) VALUES (@Front, @Back, @StackID);";
        connection.Execute(sql, parameters);
    }

    public int AddStack(string name)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var parameters = new { Name = name };
        string stackQuery = @"
                IF NOT EXISTS (SELECT Id FROM stacks WHERE name = @Name) 
                    BEGIN 
                        INSERT INTO stacks (name) VALUES (@Name); 
                    END; 
                SELECT Id FROM stacks WHERE name = @Name;";
        int stackId = connection.QuerySingle<int>(stackQuery, parameters);

        return stackId;
    }

    public void AddFakeData(string filePath)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var sql = File.ReadAllText(filePath);
        connection.Execute(sql);
    }

    public void AddStudySession(StudySessionRecord record)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var parameters = new { Date = record.Date, Score = record.Score, Questions = record.Questions, StackID = record.StackID };
        string sql = "INSERT INTO study_sessions (date, score, questions, stackID) VALUES (@Date, @Score, @Questions, @StackID)";
        connection.Execute(sql, parameters);
    }

    public List<Stack> GetAllStackNames()
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        string getStackNames = @"SELECT Id, name FROM stacks;";
        return connection.Query<Stack>(getStackNames).ToList();
    }

    public List<StackDTO> GetStacksAndSessionCount()
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        string sql = @"
            SELECT 
                stacks.Id,
                stacks.name,
                COUNT(study_sessions.Id) AS session_count
            FROM 
                stacks
            LEFT JOIN 
                study_sessions ON stacks.Id = study_sessions.stackId
            GROUP BY 
                stacks.Id, stacks.name;";
        return connection.Query<StackDTO>(sql).ToList();
    }

    public List<FlashCardDTO> GetFlashCardDTO(int stackID)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var parameters = new { ID = stackID };
        string sql = @"SELECT front, back FROM flashcards WHERE stackID = @ID";
        return connection.Query<FlashCardDTO>(sql, parameters).ToList();
    }

    public string GetStackName(int stackID)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var parameters = new { ID = stackID };
        string sql = "SELECT name FROM stacks WHERE Id = @ID";
        return connection.QuerySingle<string>(sql, parameters);
    }

    public List<StudySessionDTO> GetStudySessionRecords(int stackID)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var parameters = new { ID = stackID };
        string sql = @"SELECT date, score, questions FROM study_sessions WHERE StackId = @ID";
        return connection.Query<StudySessionDTO>(sql, parameters).ToList();
    }

    public void DeleteStack(int stackID)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        var parameters = new { ID = stackID };
        string sql = @"DELETE FROM stacks WHERE stacks.Id = @ID;";
        connection.Execute(sql, parameters);
    }

    public List<int> GetYears()
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        string sql = @"SELECT DISTINCT YEAR(date) FROM study_sessions";
        return connection.Query<int>(sql).ToList();
    }

    public List<StudyReport> GetStudyReport(int year, string filePath)
    {
        using var connection = new SqlConnection(AppConfig.ConnectionString);
        string sql = File.ReadAllText(filePath);
        var parameters = new { Year = year };
        return connection.Query<StudyReport>(sql, parameters).ToList();
    }
}