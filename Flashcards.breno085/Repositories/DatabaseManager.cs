using Dapper;
using Microsoft.Data.SqlClient;

namespace flashcards.Repositories
{
    public class DatabaseManager
    {
        private readonly string connectionString = "Server=localhost,1433;Database=Flashcards;User Id=sa;Password=S3cureP@ssw0rd2024#;TrustServerCertificate=true";

        public void CreateTables()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var createStacksTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stacks' AND xtype='U')
                CREATE TABLE Stacks (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    LanguageName VARCHAR(255) NOT NULL
                );";

                connection.Execute(createStacksTableSql);

                var createFlashcardsTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Flashcards' AND xtype='U')
                CREATE TABLE Flashcards (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Front VARCHAR(MAX) NOT NULL,
                    Back VARCHAR(MAX) NOT NULL,
                    StackId INT,
                    FOREIGN KEY (StackId) REFERENCES Stacks(Id)
                );";

                connection.Execute(createFlashcardsTableSql);

                var createStudyTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Study' AND xtype='U')
                CREATE TABLE Study (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Date DATE,
                    Score INT,
                    StackId INT,
                    FOREIGN KEY (StackId) REFERENCES Stacks(Id)
                );";

                connection.Execute(createStudyTableSql);
            }
        }
    }
}
