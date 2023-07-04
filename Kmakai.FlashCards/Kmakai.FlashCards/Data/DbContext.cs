using System.Configuration;
using System.Data.SqlClient;

namespace Kmakai.FlashCards.Data;

public class DbContext
{
    private readonly string? ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");
    private readonly string? ServerConnectionString = ConfigurationManager.AppSettings.Get("serverConnectionString");

    public void CreateDatabase()
    {
        if (ServerConnectionString == null)
        {
            throw new Exception("Connection string is null");
        }

        using (SqlConnection connection = new SqlConnection(ServerConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @" 
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FlashCardsApp')
                BEGIN
                    CREATE DATABASE FlashCardsApp;
                END";

            command.ExecuteNonQuery();

            connection.Close();
        }

        Console.WriteLine("Database created");
    }

    public void CreateTables()
    {
        if (ConnectionString == null)
        {
            throw new Exception("Connection string is null");
        }

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @" 
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
                BEGIN
                    CREATE TABLE Stacks (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(50) NOT NULL UNIQUE
                    );
                END
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Flashcards')
                BEGIN
                    CREATE TABLE Flashcards (
                        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        StackId INT NOT NULL,
                        Front NVARCHAR(50) NOT NULL,
                        Back NVARCHAR(50) NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                    );
                END
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessions')
                BEGIN
                    CREATE TABLE StudySessions (
                        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        StackId INT NOT NULL,
                        Score INT NOT NULL,
                        Date DATE NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                    );
                END";

            command.ExecuteNonQuery();
            connection.Close();
        }

        Console.WriteLine("Tables created");
    }
    public void InitializeDatabase()
    {
        CreateDatabase();
        CreateTables();
        Console.WriteLine("Database initialized");
    }

}
