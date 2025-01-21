using Microsoft.Data.SqlClient;

namespace FlashCards.Database;

internal static class DatabaseCreation
{
    private static readonly string serverConnectionString = "Server=LAPTOP-LFCOM607;Integrated Security=true;TrustServerCertificate=True;";
    public static readonly string dataBaseName = "FlashCardsDB";
    public static readonly string dbConnectionString = $"Server=LAPTOP-LFCOM607;Database={dataBaseName};Integrated Security=true;TrustServerCertificate=True;";
    private static readonly string stackTableName = "Stacks";
    private static readonly string flashCardTableName = "FlashCards";
    private static readonly string studySessionTableName = "StudySessions";

    internal static void CreateDatabase()
    {

        using (SqlConnection connection = new SqlConnection(serverConnectionString))
        {
            connection.Open();
            string checkDbQuery = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{dataBaseName}') CREATE DATABASE [{dataBaseName}]";
            using (SqlCommand command = new SqlCommand(checkDbQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        CreateTables();
    }

    private static void CreateTables()
    {
        string tableConnectionString = $"{dbConnectionString};";
        using (SqlConnection connection = new SqlConnection(tableConnectionString))
        {
            connection.Open();
            string createTableQuery = $@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{stackTableName}')
            CREATE TABLE {stackTableName} (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(MAX) NOT NULL
            )";
            using (SqlCommand command = new SqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            createTableQuery = $@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{flashCardTableName}')
            CREATE TABLE {flashCardTableName} (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Question NVARCHAR(MAX) NOT NULL,
                Answer NVARCHAR(MAX) NOT NULL,
                StackId INT FOREIGN KEY REFERENCES Stacks(Id)
                ON DELETE CASCADE
                ON UPDATE CASCADE
            )";
            using (SqlCommand command = new SqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            createTableQuery = $@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{studySessionTableName}')
            CREATE TABLE {studySessionTableName} (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Date DATETIME2 NOT NULL,
                Score NVARCHAR(MAX) NOT NULL,
                StackName NVARCHAR(MAX) NOT NULL,
                StackId INT FOREIGN KEY REFERENCES Stacks(Id)
                ON DELETE CASCADE
                ON UPDATE CASCADE
            )";
            using (SqlCommand command = new SqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
