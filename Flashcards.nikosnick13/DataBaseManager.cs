using Dapper;
using Microsoft.Data.SqlClient;
using static System.Console;

namespace Flashcards.nikosnick13;

internal class DataBaseManager
{
    public void CreateTables(string connectionString)
    {
        CreateTableStack(connectionString);
        CreateTableFlashcards(connectionString);
        CreateTableStudySessions(connectionString);

    }

    private void CreateTableStack(string connectionString)
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                BEGIN
                    CREATE TABLE Stacks(
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(255) NOT NULL
                    );
                END";

            conn.Execute(query);
        }
        catch (Exception ex)
        {
            WriteLine($"Error creating table: {ex.Message}");

        }
    }

    private void CreateTableFlashcards(string connectionString)
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"
                IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'Flashcards')
                BEGIN
                    CREATE TABLE Flashcards(
                        Id INT IDENTITY(1,1)  PRIMARY KEY,
                        Question  NVARCHAR(255) NOT NULL,
                        Answer  NVARCHAR(255) NOT NULL,
                        Stack_Id  INT NOT NULL,
                        FOREIGN KEY(Stack_Id) REFERENCES Stacks(Id) ON DELETE CASCADE                 
                        )
                END";

            conn.Execute(query);
        }
        catch (Exception ex)
        {
            WriteLine($"Error creating table: {ex.Message}");

        }
    }

    private void CreateTableStudySessions(string connectionString)
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                BEGIN
                    CREATE TABLE StudySessions(
                        Id INT IDENTITY(1,1) PRIMARY KEY, 
                        Score INT,
                        Date DATETIME,
                        Stack_Id  INT NOT NULL,
                        FOREIGN KEY(Stack_Id)  REFERENCES Stacks(Id) ON DELETE CASCADE
                        )          
                END";

            conn.Execute(query);
        }
        catch (Exception ex)
        {
            WriteLine($"Error creating table: {ex.Message}");
        }
    }
}