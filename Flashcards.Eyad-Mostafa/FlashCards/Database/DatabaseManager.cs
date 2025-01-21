using Microsoft.Data.SqlClient;
using FlashCards.Models;

namespace FlashCards.Database;

internal static class DatabaseManager
{
    private static readonly string _connectionString = "Server=localhost\\MSSQLSERVER08;Trusted_Connection=True;TrustServerCertificate=True;";
    private static readonly string _databaseConnectionString = "Server=localhost\\MSSQLSERVER08;Database=FlashCards;Trusted_Connection=True;TrustServerCertificate=True;";

    public static void Start()
    {
        CreateDatabase();
        CreateTabels();
    }

    private static void CreateDatabase()
    {
        using var connection = new SqlConnection(_connectionString);
        try
        {
            connection.Open();

            string createDatabaseQuery = @" IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FlashCards')
                    BEGIN
                        CREATE DATABASE FlashCards;
                    END;
                ";

            using (var command = new SqlCommand(createDatabaseQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void CreateTabels()
    {
        string createStacksTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Stacks' AND xtype = 'U')
            BEGIN
                CREATE TABLE Stacks (
                    StackID INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) UNIQUE NOT NULL
                );
            END;
        ";

        string createFlashcardsTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Flashcards' AND xtype = 'U')
            BEGIN
                CREATE TABLE Flashcards (
                    FlashcardID INT IDENTITY(1,1) PRIMARY KEY,
                    StackID INT NOT NULL,
                    Question NVARCHAR(255) NOT NULL,
                    Answer NVARCHAR(255) NOT NULL,
                    FOREIGN KEY (StackID) REFERENCES Stacks(StackID) ON DELETE CASCADE
                );
            END;
        ";

        string createStudySessionsTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'StudySessions' AND xtype = 'U')
            BEGIN
                CREATE TABLE StudySessions (
                    SessionID INT IDENTITY(1,1) PRIMARY KEY,
                    StackID INT NOT NULL,
                    Date DATE NOT NULL,
                    Score FLOAT NOT NULL,
                    FOREIGN KEY (StackID) REFERENCES Stacks(StackID) ON DELETE CASCADE
                );
            END;
        ";

        ExecuteSqlScript(_databaseConnectionString, createStacksTable);
        ExecuteSqlScript(_databaseConnectionString, createFlashcardsTable);
        ExecuteSqlScript(_databaseConnectionString, createStudySessionsTable);
    }

    private static void ExecuteSqlScript(string connectionString, string script)
    {
        using var connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();

            using var command = new SqlCommand(script, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    private static List<T> ProcessReader<T>(SqlDataReader reader, Func<SqlDataReader, T> mapFunction)
    {
        var items = new List<T>();

        while (reader.Read())
        {
            items.Add(mapFunction(reader));
        }

        return items;
    }

    public static List<Stack> GetStacks()
    {
        var stacks = new List<Stack>();
        string script = "SELECT * FROM Stacks";

        try
        {
            using var connection = new SqlConnection(_databaseConnectionString);
            connection.Open();

            using var command = new SqlCommand(script, connection);
            var reader = command.ExecuteReader();
            stacks = ProcessReader(reader, MapStack);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }

        return stacks;
    }

    private static Stack MapStack(SqlDataReader reader)
    {
        return new Stack
        {
            StackId = (int)reader["StackID"],
            Name = (string)reader["Name"]
        };
    }

    public static void AddStack(Stack stack)
    {
        using var connection = new SqlConnection(_databaseConnectionString);
        try
        {
            connection.Open();

            string script = "INSERT INTO Stacks (Name) VALUES (@Name)";

            using (var command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@Name", stack.Name);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    public static void DeleteStack(Stack stack)
    {
        using var connection = new SqlConnection(_databaseConnectionString);
        try
        {
            connection.Open();

            string script = "DELETE FROM Stacks WHERE StackID = @StackID";

            using (var command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@StackID", stack.StackId);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    internal static List<Flashcard> GetFlashcards(int stackId)
    {
        var flashcards = new List<Flashcard>();
        string script = "SELECT * FROM Flashcards WHERE StackID = @StackID";

        try
        {
            using var connection = new SqlConnection(_databaseConnectionString);
            connection.Open();

            using var command = new SqlCommand(script, connection);
            command.Parameters.AddWithValue("@StackID", stackId);

            var reader = command.ExecuteReader();
            flashcards = ProcessReader(reader, MapFlashcard);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }

        return flashcards;
    }

    private static Flashcard MapFlashcard(SqlDataReader reader)
    {
        return new Flashcard
        {
            FlashcardId = (int)reader["FlashcardID"],
            StackId = (int)reader["StackID"],
            Question = (string)reader["Question"],
            Answer = (string)reader["Answer"]
        };
    }

    public static void AddFlashcard(Flashcard flashcard)
    {
        string script = "INSERT INTO Flashcards (StackID, Question, Answer) VALUES (@StackID, @Question, @Answer)";

        using var connection = new SqlConnection(_databaseConnectionString);
        try
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@StackID", flashcard.StackId);
                command.Parameters.AddWithValue("@Question", flashcard.Question);
                command.Parameters.AddWithValue("@Answer", flashcard.Answer);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    public static void DeleteFlashcard(Flashcard flashcard)
    {
        using SqlConnection connection = new SqlConnection(_databaseConnectionString);
        try
        {
            connection.Open();
            string script = "DELETE FROM Flashcards WHERE FlashcardID = @FlashcardID";
            using (SqlCommand command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@FlashcardID", flashcard.FlashcardId);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    public static void EditFlashcard(Flashcard newFlashcard)
    {
        using var connection = new SqlConnection(_databaseConnectionString);

        try
        {
            connection.Open();

            string script = "UPDATE Flashcards SET Question = @Question, Answer = @Answer WHERE FlashcardID = @FlashcardID";
            using (var command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@FlashcardID", newFlashcard.FlashcardId);
                command.Parameters.AddWithValue("@Question", newFlashcard.Question);
                command.Parameters.AddWithValue("@Answer", newFlashcard.Answer);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    public static void AddStudySession(StudySession session)
    {

        using var connection = new SqlConnection(_databaseConnectionString);
        try
        {
            connection.Open();

            string script = "INSERT INTO StudySessions (StackID, Date, Score) VALUES (@StackID, @Date, @Score)";
            using (var command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@StackID", session.StackId);
                command.Parameters.AddWithValue("@Date", session.SessionDate);
                command.Parameters.AddWithValue("@Score", session.Score);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    public static List<StudySession> GetStudySessions()
    {
        using var connection = new SqlConnection(_databaseConnectionString);
        try
        {
            connection.Open();

            string script = "SELECT * FROM StudySessions";
            using (var command = new SqlCommand(script, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var sessions = new List<StudySession>();
                    while (reader.Read())
                    {
                        sessions.Add(new StudySession
                        {
                            StudySessionId = (int)reader["SessionID"],
                            StackId = (int)reader["StackID"],
                            SessionDate = (DateTime)reader["Date"],
                            Score = (int)reader["Score"]
                        });
                    }
                    return sessions;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
            return new List<StudySession>();
        }
    }
}
