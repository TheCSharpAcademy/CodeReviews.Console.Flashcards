using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;

namespace vcesario.Flashcards;

public static class DataService
{
    public static void DebugDeleteDatabase()
    {
        string? masterConnectionString = ConfigurationManager.AppSettings.Get("masterConnectionString");
        string? databaseName = ConfigurationManager.AppSettings.Get("databaseName");
        using (SqlConnection connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();

            try
            {
                string checkDbQuery = $"DROP DATABASE {databaseName}";
                connection.Execute(checkDbQuery);
                Console.WriteLine("Database dropped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.ReadLine();
        }
    }

    public static void Initialize()
    {
        bool isNewDb = false;
        string? databaseName = ConfigurationManager.AppSettings.Get("databaseName");
        string? masterConnectionString = ConfigurationManager.AppSettings.Get("masterConnectionString");

        Console.WriteLine("Checking database...");
        using (SqlConnection connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();

            string checkDbQuery = "SELECT database_id FROM sys.databases WHERE name = @DatabaseName";
            var result = connection.QueryFirstOrDefault(checkDbQuery, new { DatabaseName = databaseName });

            if (result == null)
            {
                string createDbQuery = $"CREATE DATABASE {databaseName}";
                connection.Execute(createDbQuery);
                Console.WriteLine($"Database '{databaseName}' created.");
                isNewDb = true;
            }
            else
            {
                Console.WriteLine($"Database '{databaseName}' found.");
            }
        }

        if (!isNewDb)
        {
            return;
        }

        Console.WriteLine("Creating table...");
        using (var connection = OpenConnection())
        {
            try
            {
                string createTablesQuery = @"
                    CREATE TABLE Stacks(
                        Id INT PRIMARY KEY IDENTITY,
                        Name VARCHAR(50) UNIQUE
                    );
                    
                    CREATE TABLE Cards(
                        Id INT PRIMARY KEY IDENTITY,
                        StackId INT FOREIGN KEY REFERENCES Stacks(Id)
                            ON DELETE CASCADE,
                        Front VARCHAR(50),
                        Back VARCHAR(50)
                    );
                    
                    CREATE TABLE StudySessions(
                        Id INT PRIMARY KEY IDENTITY,
                        StackId INT FOREIGN KEY REFERENCES Stacks(Id)
                            ON DELETE CASCADE,
                        Date DATETIME2,
                        Score FLOAT(2)
                    );";

                connection.Execute(createTablesQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
            }
        }
    }

    public static SqlConnection OpenConnection()
    {
        string? dbConnectionString = ConfigurationManager.AppSettings.Get("dbConnectionString");
        var connection = new SqlConnection(dbConnectionString);
        connection.Open();

        return connection;
    }
}