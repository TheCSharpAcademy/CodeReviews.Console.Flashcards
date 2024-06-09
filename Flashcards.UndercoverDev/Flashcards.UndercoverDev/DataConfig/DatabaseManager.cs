using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Flashcards.UndercoverDev.DataConfig
{
    public class DatabaseManager : IDatabaseManager
    {
        const string _databaseName = "FlashcardDB";
        private readonly string _connectionStringTemplate = ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
        private readonly string _masterConnectionString;
        private readonly string _connectionString;

        public DatabaseManager()
        {
            _masterConnectionString = string.Format(_connectionStringTemplate, "master");
            _connectionString = string.Format(_connectionStringTemplate.Replace("Database=master;", $"Database={_databaseName};"), _databaseName);
        }

        public void InitializeDatabase()
        {
            using var connection = new SqlConnection(_masterConnectionString);
            connection.Open();
            string createDatabaseQuery = $"IF DB_ID('{_databaseName}') IS NULL CREATE DATABASE {_databaseName};";
            connection.Execute(createDatabaseQuery);
        }

        public void CreateFlashcardsTables()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string createFlashcardsTableQuery = @$"
                IF OBJECT_ID('Flashcards') IS NULL
                CREATE TABLE Flashcards(
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    StackId INT NOT NULL,
                    Question VARCHAR(255) NOT NULL,
                    Answer VARCHAR(255) NOT NULL,
                    FOREIGN KEY (StackId) REFERENCES Stack(Id) ON DELETE CASCADE
                );";
            connection.Execute(createFlashcardsTableQuery);
        }

        public void CreateStacksTables()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string createStackTableQuery = @$"
                IF OBJECT_ID('Stack') IS NULL
                CREATE TABLE Stack(
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name VARCHAR(255) NOT NULL UNIQUE
                );";
            connection.Execute(createStackTableQuery);
        }

        public void CreateSessionsTables()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string createStudySessionsTableQuery = @"
                IF OBJECT_ID('StudySessions') IS NULL
                CREATE TABLE StudySessions(
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    StackId INT NOT NULL,
                    SessionDate DATETIME NOT NULL,
                    Score INT NOT NULL,
                    TotalQuestions INT NOT NULL,
                    FOREIGN KEY (StackId) REFERENCES Stack (Id) ON DELETE CASCADE
        );";
            connection.Execute(createStudySessionsTableQuery);
        }
    }
}