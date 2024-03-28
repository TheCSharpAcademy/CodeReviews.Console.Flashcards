using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards;

public class DataAccess
{
    public string ConnectionString { get; set; }
    public string InitialConnectionString { get; set; }
    public DataAccess()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"] ?? "Connectionstring not found";
        InitialConnectionString = configuration.GetSection("ConnectionStrings")["InitialConnection"] ?? "Connectionstring not found";
    }

    public void CreateDatabase()
    {
        string sql =
        @"
            IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Flashcards')
            BEGIN
                CREATE DATABASE [Flashcards]
            END
        ";

        try
        {
            using SqlConnection connection = new(InitialConnectionString);
            connection.Open();
            connection.Execute(sql);
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]Unable to connect to database.[/] Exiting program...");
            Environment.Exit(0);
        }
    }

    public void InitializeTables()
    {
        string createStacks =
        @"
            IF OBJECT_ID('Stacks', 'U') IS NULL
            BEGIN
            CREATE TABLE dbo.Stacks
            (
                Id INT PRIMARY KEY IDENTITY(1, 1),
                Name VARCHAR(100) NOT NULL UNIQUE
            );
            END;
        ";

        string createFlashcards =
        @"
            IF OBJECT_ID('Flashcards', 'U') IS NULL
            BEGIN
            CREATE TABLE dbo.Flashcards
            (
                Id INT PRIMARY KEY IDENTITY(1, 1),
                Question VARCHAR(255) NOT NULL,
                Answer VARCHAR(255) NOT NULL,
                StackId INT NOT NULL FOREIGN KEY REFERENCES Stacks(Id) ON DELETE CASCADE 
            );
            END;
        ";

        string createStudySessions =
        @"
            IF OBJECT_ID('StudySessions', 'U') IS NULL
            BEGIN
            CREATE TABLE dbo.StudySessions
            (
                Id INT PRIMARY KEY IDENTITY(1, 1),
                Score INT NOT NULL,
                Date DATETIME NOT NULL,
                StackId INT NOT NULL FOREIGN KEY REFERENCES Stacks(Id)
            );
            END;
        ";

        try
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            connection.Execute(createStacks);
            connection.Execute(createFlashcards);
            connection.Execute(createStudySessions);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]There was a problem creating the tables: {ex.Message}[/]");
        }
    }

    public IEnumerable<DTOs.Flashcard> GetFlashcards()
    {
        string sql =
        @"
            SELECT *
            FROM Flashcards;
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        return connection.Query<DTOs.Flashcard>(sql);
    }

    public IEnumerable<DTOs.Flashcard> GetFlashcardsByStackId(int id)
    {
        var parameters = new { Id = id };
        string sql =
        @"
            SELECT *
            FROM Flashcards
            WHERE StackId = @Id;
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        return connection.Query<DTOs.Flashcard>(sql, parameters);
    }

    public bool AddFlashcard(Flashcard flashcard)
    {
        var parameters = new { flashcard.Answer, flashcard.Question, flashcard.StackId };

        string sql =
        @"
            INSERT INTO Flashcards (Answer, Question, StackId)
            VALUES (@Answer, @Question, @StackId);
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        int result = connection.Execute(sql, parameters);

        return result == 1;
    }

    public bool RemoveFlashcard(int id)
    {
        var parameters = new { Id = id };

        string sql =
        @"
            DELETE FROM Flashcards
            WHERE Id = @Id;
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        int result = connection.Execute(sql, parameters);

        return result == 1;
    }

    public IEnumerable<Stack> GetStacks()
    {
        string sql =
        @"
            SELECT *
            FROM Stacks;
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        return connection.Query<Stack>(sql);
    }

    public bool AddStack(Stack stack)
    {
        var parameters = new { stack.Name };

        CheckIfStackExists(stack);

        string insert =
        @"
            INSERT INTO Stacks (Name)
            VALUES (@Name);
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        int result = connection.Execute(insert, parameters);

        return result == 1;
    }

    public bool CheckIfStackExists(Stack stack)
    {
        var parameters = new { stack.Name };

        string checkExisting =
        @"
            SELECT COUNT(*)
            FROM Stacks
            WHERE Name = @Name;
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        int count = connection.ExecuteScalar<int>(checkExisting, parameters);

        return count > 0;
    }

    public bool RemoveStack(int id)
    {
        var parameters = new { Id = id };

        string sql =
        @"
            DELETE FROM Stacks
            WHERE Id = @Id;
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        int result = connection.Execute(sql, parameters);

        return result == 1;
    }

    public bool AddStudySession(StudySession studySession)
    {
        var parameters = new { studySession.Score, studySession.Date, studySession.StackId };

        string sql =
        @"
            INSERT INTO StudySessions (Score, Date, StackId)
            VALUES (@Score, @Date, @StackId);
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        int result = connection.Execute(sql, parameters);

        return result == 1;
    }

    public IEnumerable<StudyReport> GetStudyReport(int year)
    {
        var parameters = new { Year = year };
        string sql =
        @"
            SELECT Stacks.Name, 
                COALESCE(PivotTable.[1], 0) as January, 
                COALESCE(PivotTable.[2], 0) as February, 
                COALESCE(PivotTable.[3], 0) as March, 
                COALESCE(PivotTable.[4], 0) as April, 
                COALESCE(PivotTable.[5], 0) as May, 
                COALESCE(PivotTable.[6], 0) as June, 
                COALESCE(PivotTable.[7], 0) as July, 
                COALESCE(PivotTable.[8], 0) as August, 
                COALESCE(PivotTable.[9], 0) as September, 
                COALESCE(PivotTable.[10], 0) as October, 
                COALESCE(PivotTable.[11], 0) as November, 
                COALESCE(PivotTable.[12], 0) as December
            FROM
            (
                SELECT StackId, Score, MONTH(Date) as Month
                FROM StudySessions
                WHERE YEAR(Date) = @Year
            ) AS SourceTable
            PIVOT
            (
                AVG(Score)
                FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
            ) AS PivotTable

            INNER JOIN Stacks
            ON PivotTable.StackId = Stacks.Id;
        ";

        using SqlConnection connection = new(ConnectionString);
        connection.Open();
        return connection.Query<StudyReport>(sql, parameters);
    }
}