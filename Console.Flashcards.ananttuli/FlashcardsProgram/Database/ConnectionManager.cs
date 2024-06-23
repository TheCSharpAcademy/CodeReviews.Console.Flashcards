using System.Security;
using Dapper;
using FlashcardsProgram.Stacks;
using Microsoft.Data.SqlClient;

namespace FlashcardsProgram.Database;

public class ConnectionManager
{
    private static SqlConnection? _connection;

    public static SqlConnection Connection
    {
        get
        {
            return _connection ?? throw new Exception("Missing database connection");
        }

        private set
        {
            _connection = value;
        }
    }

    private static void CreateDatabaseIfNotExists(string databaseName)
    {
        string connectionString = GetConnectionString(null);

        using (var dbCreateConnection = new SqlConnection(connectionString))
        {
            dbCreateConnection.Open();

            dbCreateConnection.Execute(
                $@"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}')
                BEGIN
                    CREATE DATABASE {databaseName};
                END
                "
            );

            dbCreateConnection.Close();
        }
    }

    private static string GetConnectionString(string? databaseName)
    {
        var dbConfig = ConfigManager.Config.GetSection("Database");

        string databaseInfo = databaseName != null ? $"Database={databaseName};" : "";

        return $"Server={dbConfig["Host"]};" +
                databaseInfo +
                $"Integrated Security=True;" +
                $"TrustServerCertificate=True;";
    }

    private static void DropTables()
    {
        Connection.Execute($@"
            DROP TABLE Flashcards;
            DROP TABLE Stacks;
        ");
    }



    public static void Init()
    {
        try
        {
            var dbConfig = ConfigManager.Config.GetSection("Database");
            string databaseName = dbConfig["Name"] ?? throw new Exception("Database name must be configured in appsettings.json");

            CreateDatabaseIfNotExists(databaseName);

            Connection = new SqlConnection(
                GetConnectionString(databaseName)
            );

            Connection.Open();

            DropTables();
            Migration.CreateTablesIfNotExists(Connection);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection could not be initialized. {ex.Message}");
            Environment.Exit(1);
        }
    }
}