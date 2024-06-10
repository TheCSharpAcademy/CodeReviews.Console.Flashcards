using System.Data.SqlClient;
using System.Configuration;
using Dapper;

public class DatabaseManager
{
    private string ConnectionString { get; }

    private string? dbName = ConfigurationManager.AppSettings["DatabaseName"];

    public DatabaseManager(string connectionString)
    {
        this.ConnectionString = connectionString;
    }

    public SqlConnection GetConnection() => new(ConnectionString);

    public void InitializeDB(){

        if (!DatabaseExists())
        {
            CreateDb();
            CreateTables();
        }

        else if(DatabaseExists() && !TablesExist())
        {
            CreateTables();
        }
    }

    private bool DatabaseExists()
    {
        string query = "SELECT COUNT(1) FROM sys.databases WHERE name = @DbName";

        using (var conn = GetConnection())
        {
            conn.Open();
            int count = conn.ExecuteScalar<int>(query, new { DbName = dbName });
            return count != 0;
        }
    }

    private void CreateDb()
    {
        string query = $"CREATE DATABASE {dbName}";
        using (var conn = GetConnection())
        {
            conn.Open();
            conn.Execute(query);
        }
    }

    private bool TablesExist()
    {
        string query = "SELECT COUNT(1) FROM sys.tables WHERE name IN ('Stacks', 'Flashcards', 'StudySessions')";

        using (var conn = GetConnection())
        {
            conn.Open();
            int count = conn.ExecuteScalar<int>(query);
            return count == 3;
        }
    }

    private void CreateTables()
    {
        string[] createTableQueries = new string[]
        {
            @"CREATE TABLE Stacks (
            Id INT PRIMARY KEY IDENTITY(1,1), 
            Name NVARCHAR(100) UNIQUE NOT NULL
        )",

            @"CREATE TABLE Flashcards ( 
            Id INT PRIMARY KEY IDENTITY(1,1), 
            Question NVARCHAR(1000) NOT NULL, 
            Answer NVARCHAR(1000) NOT NULL, 
            StackId INT, 
            FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
        )",

            @"CREATE TABLE StudySessions ( 
            Id INT PRIMARY KEY IDENTITY(1,1), 
            StackId INT, 
            Date DATETIME NOT NULL, 
            Score INT NOT NULL, 
            FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE 
        )"
        };

        using (var conn = GetConnection())
        {
            conn.Open();
            foreach (var query in createTableQueries)
            {
                conn.Execute(query);
            }
        }
    }
}