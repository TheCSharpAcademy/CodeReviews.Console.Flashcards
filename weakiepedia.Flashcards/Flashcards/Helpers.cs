using Spectre.Console;
using System.Data.SqlClient;
using Dapper;

using static Flashcards.Program;
using static Flashcards.Configuration;

namespace Flashcards;

internal static class Helpers
{
    internal static string GetUserInput(string message)
    {
        AnsiConsole.Markup($"{message}");
        return Console.ReadLine();
    }

    internal static void PressAnyKey()
    {
        AnsiConsole.MarkupLine("[italic]\nPress any key to continue...[/]");
        Console.ReadKey();
    }

    internal static void PrintError(string message)
    {
        AnsiConsole.MarkupLine($"[grey50]{message}[/]");
    }

    internal static void CreateTablesIfNotExists()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'stacks' AND xtype = 'U')
                    BEGIN
                        CREATE TABLE stacks 
                        (
                            stack_id INT PRIMARY KEY IDENTITY (1, 1),
                            name VARCHAR(500) UNIQUE NOT NULL
                        );
                    END;

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'flashcards' AND xtype = 'U')
                    BEGIN
                        CREATE TABLE flashcards
                        (
                            flashcard_id INT PRIMARY KEY IDENTITY (1, 1),
                            question VARCHAR(500) NOT NULL,
                            answer VARCHAR(500) NOT NULL,
                            stack_id INT NOT NULL,
                            FOREIGN KEY (stack_id) REFERENCES stacks(stack_id) ON DELETE CASCADE
                        );
                    END;

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'sessions' AND xtype = 'U')
                    BEGIN
                        CREATE TABLE sessions
                        (
                            session_id INT PRIMARY KEY IDENTITY (1, 1),
                            score INT NOT NULL,
                            date DATETIME NOT NULL,  
                            stack_id INT NOT NULL,
                            FOREIGN KEY (stack_id) REFERENCES stacks(stack_id) ON DELETE CASCADE
                        );
                    END;
                ";

                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    internal static void GetStackNames()
    {
        stackNames.Clear();
        using (var connection = new SqlConnection(GetConnectionString()))
        {
            try
            {
                var query = "SELECT name FROM stacks";
                var reader = connection.ExecuteReader(query);

                while (reader.Read())
                {
                    string stackName = reader.GetString(reader.GetOrdinal("Name"));
                    stackNames.Add(new StackShowDTO(stackName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    internal static int GetStackIdByName(string stackName)
    {
        using (var connection = new SqlConnection(GetConnectionString()))
        {
            var query = "SELECT stack_id FROM stacks WHERE name = @Name;";
            int stackId = connection.QuerySingleOrDefault<int>(query, new { Name = stackName });
            return stackId;
        }
    }
    
    internal static string GetStackNameById(int stackId)
    {
        using (var connection = new SqlConnection(GetConnectionString()))
        {
            var query = "SELECT name FROM stacks WHERE stack_id = @Id;";
            string stackName = connection.QuerySingleOrDefault<string>(query, new { Id = stackId });
            return stackName;
        }
    }

    internal static void GetStudySessions()
    {
        sessions.Clear();
        using (var connection = new SqlConnection(GetConnectionString()))
        {
            try
            {
                var query = "SELECT * FROM sessions";
                var reader = connection.ExecuteReader(query);

                while (reader.Read())
                {
                    int sessionId = reader.GetInt32(reader.GetOrdinal("session_id"));
                    int score = reader.GetInt32(reader.GetOrdinal("score"));
                    DateTime date = reader.GetDateTime(reader.GetOrdinal("date"));
                    int stackId = reader.GetInt32(reader.GetOrdinal("stack_id"));
                    sessions.Add(new Session(sessionId, score, date, stackId));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}