using System.Data.SqlClient;
using Spectre.Console;

namespace Flashcards.empty_codes.Data;

internal class Database
{
    public readonly string connectionString;

    public Database(string connString)
    {
        connectionString = connString;
    }

    public void InitializeDatabase()
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string createStacksTable = @"
            CREATE TABLE IF NOT EXISTS Stacks (
                StackId INT PRIMARY KEY IDENTITY(1,1),
                StackName VARCHAR(255) NOT NULL UNIQUE
            )";

        string createFlashcardsTable = @"
            CREATE TABLE IF NOT EXISTS Flashcards (
                FlashcardId INT PRIMARY KEY IDENTITY(1,1),
                Question VARCHAR(255),
                Answer VARCHAR(255),
                StackId INT,
                FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE
            )";

        string createStudySessionsTable = @"
            CREATE TABLE IF NOT EXISTS StudySessions (
                SessionId INT PRIMARY KEY IDENTITY(1,1),
                StudyDate DATETIME,
                Score VARCHAR(255),
                StackId INT,
                FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE
            )";

        try
        {
            using var stacksTableCommand = new SqlCommand(createStacksTable, conn);
            stacksTableCommand.ExecuteNonQuery();

            using var flashcardsTableCommand = new SqlCommand(createFlashcardsTable, conn);
            flashcardsTableCommand.ExecuteNonQuery();

            using var studysessionsTableCommand = new SqlCommand(createStudySessionsTable, conn);
            studysessionsTableCommand.ExecuteNonQuery();
        }
        catch (SqlException e)
        {
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to create the database Table\n - Details: {e.Message}[/]");
            }   
        }
        AnsiConsole.MarkupLine($"[green]Database file successfully created.[/] [green]The database is ready to use.[/]");
        Console.Clear();
    }
}
