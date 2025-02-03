using Flashcards.jollejonas.DTOs;
using Flashcards.jollejonas.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.jollejonas.Data;

public class DatabaseManager(string connectionString)
{
    private readonly string _connectionString = connectionString;

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public void ExecuteNonQuery(string query)
    {
        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    public List<CardStack> GetCardStacks()
    {
        var stacks = new List<CardStack>();
        var query = "SELECT * FROM Stacks";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var stack = new CardStack
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2)
            };
            stacks.Add(stack);
        }

        if (stacks.Count == 0)
        {
            Console.WriteLine("No stacks found");
            return null;
        }
        return stacks;
    }

    public List<Card> GetCards()
    {
        var cards = new List<Card>();
        var query = "SELECT * FROM Cards";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var card = new Card
            {
                Id = reader.GetInt32(0),
                Question = reader.GetString(1),
                Answer = reader.GetString(2),
                CardStackId = reader.GetInt32(3)
            };
            cards.Add(card);
        }
        if (cards.Count == 0)
        {
            Console.WriteLine("No cards found");
            return null;
        }
        return cards;
    }

    public void UpdateCard(string question, string answer, int cardId)
    {
        var query = "UPDATE Cards SET Question = @question, Answer = @answer WHERE Id = @id";

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
        var query = "DELETE FROM Cards WHERE Id = @id";

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
        var query = "INSERT INTO Cards (Question, Answer, StackId) VALUES (@question, @answer, @stackId)";

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

    public void UpdateStack(string name, string description, int stackId)
    {
        var query = "UPDATE Stacks SET Name = @name, Description = @description WHERE Id = @id";

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
    public void CreateStack(string name, string description)
    {
        var query = "INSERT INTO Stacks (Name, Description) VALUES (@name, @description)";

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

    public void RegisterStudySession(int stackId, int correctAnswers, int wrongAnswers)
    {
        var query = "INSERT INTO StudySessions (StackId, EndTime, CorrectAnswers, WrongAnswers) VALUES (@stackId, GETDATE(), @correctAnswers, @wrongAnswers)";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@stackId", stackId);
                command.Parameters.AddWithValue("@correctAnswers", correctAnswers);
                command.Parameters.AddWithValue("@wrongAnswers", wrongAnswers);
                command.ExecuteNonQuery();
            }
        }
    }
    public CardStackDetailsDto GetCardStackDTOs(int stackId)
    {
        var cardStackDetails = new CardStackDetailsDto
        {
            Cards = new List<CardsDto>()
        };

        var query = $@"
                SELECT 
                    Stacks.Name,
                    Cards.Id,
                    Cards.Question, 
                    Cards.Answer
                FROM Stacks
                JOIN Cards ON Stacks.Id = Cards.StackId
                WHERE Stacks.Id = {stackId}
            ";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        int presentationId = 1;

        while (reader.Read())
        {
            if (cardStackDetails.CardStackName == null)
            {
                cardStackDetails.CardStackName = reader.GetString(0);
            }
            var cards = new CardsDto
            {
                Id = reader.GetInt32(1),
                PresentationId = presentationId++,
                Question = reader.GetString(2),
                Answer = reader.GetString(3)
            };
            cardStackDetails.Cards.Add(cards);
        }

        if (cardStackDetails.CardStackName == null)
        {
            Console.WriteLine("No cards found");
            return null;
        }
        return cardStackDetails;
    }

    public List<StudySessionPivotDto> GetStudySessionsPerMonth(int year)
    {
        var studySessions = new List<StudySessionPivotDto>();
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
            var session = new StudySessionPivotDto
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
    public List<StudySessionPivotDto> GetAverageCorrectAnswersPerStudySessionPerMonth(int year)
    {
        var studySessions = new List<StudySessionPivotDto>();
        string query = $@"
            WITH SessionData AS (
                SELECT
                    ss.Id,
                    s.Name,
                    ss.CorrectAnswers,
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
                    AVG(CorrectAnswers) AS AverageCorrectAnswers
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
                AVG(AverageCorrectAnswers)
                FOR SessionMonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
            ) AS PivotTable
            ORDER BY Name;";

        using var connection = GetConnection();
        connection.Open();
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var session = new StudySessionPivotDto
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
    public void EnsureDatabaseExists(string DBNAME)
    {
        var query = $@"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{DBNAME}')
                BEGIN
                    CREATE DATABASE {DBNAME}
                END
            ";

        ExecuteNonQuery(query);

        EnsureTablesExist();
    }
    public void EnsureTablesExist()
    {
        var query = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
                BEGIN
                    CREATE TABLE Stacks (
                        Id INT PRIMARY KEY IDENTITY,
                        Name NVARCHAR(100) NOT NULL,
                        Description NVARCHAR(1000) NOT NULL
                    )
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Cards')
                BEGIN
                    CREATE TABLE Cards (
                        Id INT PRIMARY KEY IDENTITY,
                        Question NVARCHAR(1000) NOT NULL,
                        Answer NVARCHAR(1000) UNIQUE NOT NULL,
                        StackId INT NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                    )
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessions')
                BEGIN
                    CREATE TABLE StudySessions (
                        Id INT PRIMARY KEY IDENTITY,
                        StackId INT NOT NULL,
                        EndTime DATETIME NOT NULL,
                        CorrectAnswers INT NOT NULL,
                        WrongAnswers INT NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                    )
                END
            ";

        ExecuteNonQuery(query);
        SeedData();
    }

    public void SeedData()
    {
        string query = @"
            IF NOT EXISTS (SELECT * FROM Stacks WHERE Name = 'C# Basics')
            BEGIN
                INSERT INTO Stacks (Name, Description) VALUES ('C# Basics', 'This stack contains basic questions about C#')
            END

            IF NOT EXISTS (SELECT * FROM Stacks WHERE Name = 'SQL Server Essentials')
            BEGIN
                INSERT INTO Stacks (Name, Description) VALUES ('SQL Server Essentials', 'This stack contains essential questions about SQL Server')
            END

            IF NOT EXISTS (SELECT * FROM Stacks WHERE Name = 'General Knowledge')
            BEGIN
                INSERT INTO Stacks (Name, Description) VALUES ('General Knowledge', 'This stack contains general knowledge questions')
            END

            DECLARE @stack1Id INT, @stack2Id INT, @stack3Id INT

            SELECT @stack1Id = Id FROM Stacks WHERE Name = 'C# Basics'
            SELECT @stack2Id = Id FROM Stacks WHERE Name = 'SQL Server Essentials'
            SELECT @stack3Id = Id FROM Stacks WHERE Name = 'General Knowledge'

            IF NOT EXISTS (SELECT * FROM Cards WHERE StackId = @stack1Id)
            BEGIN
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the base class for all classes in C#?', 'System.Object', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the keyword for creating a class in C#?', 'class', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How do you make a class abstract?', 'Use the keyword ""abstract""', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is a collection that does not allow duplicates?', 'HashSet', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How do you create a readonly property?', 'Use the keyword ""readonly"" or ""get"" without ""set""', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the default access modifier for a class?', 'internal', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What keyword is used to inherit from a class?', 'extends', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How do you define a method in C#?', 'public int MyMethod()', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the use of the ""using"" directive?', 'To include namespaces', @stack1Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How do you handle exceptions in C#?', 'Using try-catch blocks', @stack1Id)
            END

            IF NOT EXISTS (SELECT * FROM Cards WHERE StackId = @stack2Id)
            BEGIN
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the primary key?', 'A unique identifier for a row', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What command is used to create a table?', 'CREATE TABLE', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How do you add a column to a table?', 'ALTER TABLE ... ADD COLUMN', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is a foreign key?', 'A key used to link two tables', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How do you update a row?', 'UPDATE ... SET ... WHERE ...', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the command for deleting a table?', 'DROP TABLE', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How do you retrieve all rows from a table?', 'SELECT * FROM table', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is an index?', 'A performance optimization tool', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How do you create a database?', 'CREATE DATABASE dbname', @stack2Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the command for inserting a row?', 'INSERT INTO ... VALUES ...', @stack2Id)
            END

            IF NOT EXISTS (SELECT * FROM Cards WHERE StackId = @stack3Id)
            BEGIN
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the capital of France?', 'Paris', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('Who wrote ""1984""?', 'George Orwell', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the largest planet in our solar system?', 'Jupiter', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the chemical symbol for water?', 'H2O', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('Who painted the Mona Lisa?', 'Leonardo da Vinci', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('How many continents are there?', '7', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the longest river in the world?', 'Nile', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the name of the first computer?', 'ENIAC', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the speed of light?', '299,792,458 meters per second', @stack3Id)
                INSERT INTO Cards (Question, Answer, StackId) VALUES ('What is the largest desert in the world?', 'Sahara', @stack3Id)
            END
        ";

        ExecuteNonQuery(query);
    }

    public void DropAndRecreateTables()
    {
        var dropQuery = @"
                IF OBJECT_ID('dbo.StudySessions', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.StudySessions
                END

                IF OBJECT_ID('dbo.Cards', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.Cards
                END

                IF OBJECT_ID('dbo.Stacks', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.Stacks
                END
            ";

        ExecuteNonQuery(dropQuery);
        EnsureTablesExist();
    }
}
