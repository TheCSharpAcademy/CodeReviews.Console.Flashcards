using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace cacheMe512.Flashcards;

internal static class Database
{
    private static string ConnectionString;
    private static string LogLevel;

    static Database()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        ConnectionString = configuration.GetConnectionString("FlashcardsDatabase");
        LogLevel = configuration["AppSettings:LogLevel"];
    }

    internal static SqlConnection GetConnection()
    {
        var connection = new SqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    internal static void InitializeDatabase()
    {
        var builder = new SqlConnectionStringBuilder(ConnectionString);
        var databaseName = builder.InitialCatalog;

        var masterConnectionString = new SqlConnectionStringBuilder(ConnectionString)
        {
            InitialCatalog = "master"
        }.ToString();

        using (var connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();

            var dbExists = connection.ExecuteScalar<int>(
                @"SELECT COUNT(*) 
                  FROM sys.databases 
                  WHERE name = @DatabaseName",
                new { DatabaseName = databaseName });

            if (dbExists == 0)
            {
                connection.Execute($"CREATE DATABASE [{databaseName}]");
                Console.WriteLine($"Database '{databaseName}' created.");
            }
        }

        using (var connection = GetConnection())
        {
            connection.Execute(
                @"IF OBJECT_ID('stacks', 'U') IS NULL
                  CREATE TABLE stacks (
                      Id INT PRIMARY KEY IDENTITY,
                      Name VARCHAR(50) NOT NULL,
                      CreatedDate DATETIME NOT NULL
                  );");

            connection.Execute(
                @"IF OBJECT_ID('flashcards', 'U') IS NULL
                  CREATE TABLE flashcards (
                      Id INT PRIMARY KEY IDENTITY,
                      StackId INT NOT NULL,
                      Question VARCHAR(100) NOT NULL,
                      Answer VARCHAR(100) NOT NULL,
                      FOREIGN KEY (StackId) REFERENCES stacks(Id) ON DELETE CASCADE
                  );");

            connection.Execute(
                @"IF OBJECT_ID('study_session', 'U') IS NULL
                  CREATE TABLE study_session (
                      Id INT PRIMARY KEY IDENTITY,
                      Date DATETIME NOT NULL,
                      Score INT NOT NULL
                  );");
        }
    }
}
