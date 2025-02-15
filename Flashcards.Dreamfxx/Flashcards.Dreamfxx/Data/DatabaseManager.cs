using Flashcards.Dreamfxx.Dtos;
using Flashcards.Dreamfxx.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Flashcards.Dreamfxx.Data;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public List<Stack> GetStacks()
    {
        var stacks = new List<Stack>();
        var query = "SELECT * FROM Stacks";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var stack = new Stack
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Flashcards = new List<Flashcard>()
            };
            stacks.Add(stack);
        }

        if (stacks.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No stacks found. Press any key to continue.[/]");
            Console.ReadKey();
        }
        return stacks;
    }

    public List<Flashcard> GetCards()
    {
        var cards = new List<Flashcard>();
        var query = @"
            SELECT f.*, s.Name as StackName, s.Name as StackDescription 
            FROM Flashcards f
            JOIN Stacks s ON f.StackId = s.Id";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var card = new Flashcard
            {
                Id = reader.GetInt32(0),
                Question = reader.GetString(1),
                Answer = reader.GetString(2),
                StackId = reader.GetInt32(3),
                Stack = new Stack
                {
                    Name = reader.GetString(4),
                    Description = reader.GetString(5)
                }
            };
            cards.Add(card);
        }

        if (cards.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No cards found. Press any key to continue.[/]");
            Console.ReadKey();
        }
        return cards;
    }

    public void UpdateCard(string question, string answer, int cardId)
    {
        var query = "UPDATE Flashcards SET Question = @question, Answer = @answer WHERE Id = @id";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@question", question);
                command.Parameters.AddWithValue("@answer", answer);
                command.Parameters.AddWithValue("@id", cardId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteCard(int cardId)
    {
        var query = "DELETE FROM Flashcards WHERE Id = @id";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", cardId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void CreateCard(string question, string answer, int stackId)
    {
        var query = "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@question, @answer, @stackId)";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@question", question);
                command.Parameters.AddWithValue("@answer", answer);
                command.Parameters.AddWithValue("@stackId", stackId);
                command.ExecuteNonQuery();
            }
        }
    }
    public void CreateStack(string name, string description)
    {
        var query = "INSERT INTO Stacks (Name, Name) VALUES (@name, @description)";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description", description);
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateStack(string name, string description, int stackId)
    {
        var query = "UPDATE Stacks SET Name = @name, Name = @description WHERE Id = @id";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@id", stackId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteStack(int stackId)
    {
        var query = "DELETE FROM Stacks WHERE Id = @id";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", stackId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void RegisterStudySession(int stackId, int correctAnswers, int wrongAnswers)
    {
        var query = @"
            INSERT INTO StudySessions (StackId, EndTime, CorrectAnswers, WrongAnswers) 
            VALUES (@stackId, GETDATE(), @correctAnswers, @wrongAnswers)";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@stackId", stackId);
        command.Parameters.AddWithValue("@correctAnswers", correctAnswers);
        command.Parameters.AddWithValue("@wrongAnswers", wrongAnswers);
        command.ExecuteNonQuery();
    }
    public List<SessionPivotDto> GetSessionsInMonth(int year)
    {
        var studySessions = new List<SessionPivotDto>();
        string query = $@"
            WITH SessionData AS (
                SELECT
                    ss.Id,
                    s.Name,
                    MONTH(ss.EndTime) AS SessionMonth,
                    YEAR(ss.EndTime) AS SessionYear
                FROM 
                    StudySessions ss
                INNER JOIN 
                    Stacks s ON ss.StackId = s.Id
                WHERE
                    YEAR(ss.EndTime) = {year.ToString()}
            )
            , AggregatedData AS (
                SELECT
                    Name,
                    SessionYear,
                    SessionMonth,
                    COUNT(Id) AS SessionCount
                FROM SessionData
                GROUP BY Name, SessionYear, SessionMonth
            )
            -- Step 3: Pivot the data
            SELECT
                Name,
                [1] AS January,
                [2] AS February,
                [3] AS March,
                [4] AS April,
                [5] AS May,
                [6] AS June,
                [7] AS July,
                [8] AS August,
                [9] AS September,
                [10] AS October,
                [11] AS November,
                [12] AS December
            FROM
                AggregatedData
            PIVOT
            (
                SUM(SessionCount)
                FOR SessionMonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
            ) AS PivotTable
            ORDER BY Name;";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var session = new SessionPivotDto
            {
                StackName = reader.GetString(0),
                January = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                February = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                March = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                April = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                May = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                June = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                July = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                August = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                September = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                October = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                November = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                December = reader.IsDBNull(12) ? 0 : reader.GetInt32(12)
            };
            studySessions.Add(session);
        }
        return studySessions;
    }
    public StackDto GetStackDtos(int stackId)
    {
        var query = @"
            SELECT 
                s.Id as StackId,
                s.Name as StackName,
                s.Name,
                f.Id as FlashcardId,
                f.Question,
                f.Answer
            FROM Stacks s
            LEFT JOIN Flashcards f ON s.Id = f.StackId
            WHERE s.Id = @stackId
            ORDER BY f.Id";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@stackId", stackId);
        using var reader = command.ExecuteReader();

        var stackDetails = new StackDto
        {
            StackName = null,
            FlashcardsDto = new List<FlashcardDto>()
        };

        int presentationId = 1;

        while (reader.Read())
        {
            if (stackDetails.StackName == null)
            {
                stackDetails.StackName = reader.GetString(1);
            }

            if (!reader.IsDBNull(3))
            {
                var card = new FlashcardDto
                {
                    Id = reader.GetInt32(3),
                    PresentationId = presentationId++,
                    Question = reader.GetString(4),
                    Answer = reader.GetString(5)
                };
                stackDetails.FlashcardsDto.Add(card);
            }
        }

        if (stackDetails.StackName == null)
        {
            AnsiConsole.MarkupLine("[yellow]Stack not found, press any key to continue.[/]");
            Console.ReadKey();
            return new StackDto { FlashcardsDto = new List<FlashcardDto>() };
        }

        return stackDetails;
    }




    public void EnsureDatabaseExists()
    {
        var query = @"
            IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FlashcardsDb')
            BEGIN
                CREATE DATABASE [FlashcardsDb]
            END";

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();

        EnsureTablesExist();
    }

    public void EnsureTablesExist()
    {
        var query = @"
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
    BEGIN
        CREATE TABLE Stacks (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Name NVARCHAR(100) NOT NULL,
            Name NVARCHAR(400) NOT NULL
        )
    END

    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Flashcards')
    BEGIN
        CREATE TABLE Flashcards (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Question NVARCHAR(1000) NOT NULL,
            Answer NVARCHAR(450) NOT NULL,
            StackId INT NOT NULL,
            CONSTRAINT FK_Flashcards_Stacks FOREIGN KEY (StackId) 
            REFERENCES Stacks(Id) ON DELETE CASCADE
        )
    END

    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessions')
    BEGIN
        CREATE TABLE StudySessions (
            Id INT PRIMARY KEY IDENTITY(1,1),
            StackId INT NOT NULL,
            EndTime DATETIME NOT NULL,
            CorrectAnswers INT NOT NULL,
            WrongAnswers INT NOT NULL,
            CONSTRAINT FK_StudySessions_Stacks FOREIGN KEY (StackId) 
            REFERENCES Stacks(Id) ON DELETE CASCADE
        )
    END";

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();

        SeedData();
    }



    public void SeedData()
    {
        using var connection = GetConnection();
        connection.Open();

        var checkQuery = "SELECT COUNT(*) FROM Stacks WHERE Name = 'C# Basics'";
        using var checkCommand = new SqlCommand(checkQuery, connection);
        int count = (int)checkCommand.ExecuteScalar();

        if (count == 0)
        {
            var insertStackQuery = "INSERT INTO Stacks (Name, Name) VALUES ('C# Basics', 'This stack contains basic questions about C#')";
            using var insertStackCommand = new SqlCommand(insertStackQuery, connection);
            insertStackCommand.ExecuteNonQuery();

            var getStackIdQuery = "SELECT Id FROM Stacks WHERE Name = 'C# Basics'";
            using var getStackIdCommand = new SqlCommand(getStackIdQuery, connection);
            int stackId = (int)getStackIdCommand.ExecuteScalar();

            var insertCardsQuery = @"      
                INSERT INTO Flashcards (Question, Answer, StackId) VALUES 
                ('What is the base class for all classes in C#?', 'System.Object', @stackId),
                ('What is the keyword for creating a class in C#?', 'class', @stackId),
                ('How do you make a class abstract?', 'Use the keyword ""abstract""', @stackId),
                ('What is a collection that does not allow duplicates?', 'HashSet', @stackId),
                ('How do you create a readonly property?', 'Use the keyword ""readonly"" or ""get"" without ""set""', @stackId),
                ('What is the default access modifier for a class?', 'internal', @stackId),
                ('What keyword is used to inherit from a class?', 'extends', @stackId),
                ('How do you define a method in C#?', 'public int MyMethod()', @stackId),
                ('What is the use of the ""using"" directive?', 'To include namespaces', @stackId),
                ('How do you handle exceptions in C#?', 'Using try-catch blocks', @stackId)
        ";

            using var insertCardsCommand = new SqlCommand(insertCardsQuery, connection);
            insertCardsCommand.Parameters.AddWithValue("@stackId", stackId);
            insertCardsCommand.ExecuteNonQuery();
        }


    }

    public void DropAndRecreateTables()
    {
        var dropQuery = @"
                IF OBJECT_ID('dbo.StudySessions', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.StudySessions
                END
                IF OBJECT_ID('dbo.Flashcards', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.Flashcards
                END
                IF OBJECT_ID('dbo.Stacks', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.Stacks
                END
            ";

        using var connection = new SqlConnection(_connectionString.Replace("Database=Flashcards_Data;", "FlashcardsDb"));
        connection.Open();
        using var command = new SqlCommand(dropQuery, connection);
        command.ExecuteNonQuery();

        EnsureTablesExist();
    }
}