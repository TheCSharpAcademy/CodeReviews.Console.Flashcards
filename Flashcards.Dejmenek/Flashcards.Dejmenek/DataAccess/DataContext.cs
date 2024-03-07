using Dapper;
using System.Configuration;
using System.Data.SqlClient;

namespace Flashcards.Dejmenek.DataAccess
{
    public static class DataContext
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["LocalDbConnection"].ConnectionString;

        public static void CreateDatabase()
        {
            CreateTables();
            SeedStacks();
            SeedFlashcards();
        }
        private static void CreateTables()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"IF NOT EXISTS (
                                SELECT * FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_NAME = 'Stacks'
                               )
                               BEGIN
                               CREATE TABLE Stacks (
                                   Id INT IDENTITY(1, 1) PRIMARY KEY,
                                   Name NVARCHAR(50) NOT NULL UNIQUE
                               );
                               END;

                               IF NOT EXISTS (
                                SELECT * FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_NAME = 'Flashcards'
                               )
                               BEGIN
                               CREATE TABLE Flashcards (
                                   Id INT IDENTITY(1, 1) PRIMARY KEY,
                                   StackId INT,
                                   Front NVARCHAR(50) NOT NULL,
                                   Back NVARCHAR(50) NOT NULL,
                                   FOREIGN KEY (StackId) REFERENCES Stacks(Id)
                                   ON DELETE CASCADE
                               );
                               END;

                               IF NOT EXISTS (
                                SELECT * FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_NAME = 'StudySessions'
                               )
                               BEGIN
                               CREATE TABLE StudySessions (
                                   Id INT IDENTITY(1, 1) PRIMARY KEY,
                                   StackId INT,
                                   Date DATETIME NOT NULL,
                                   Score INT NOT NULL,
                                   FOREIGN KEY (StackId) REFERENCES Stacks(Id)
                                   ON DELETE CASCADE
                               );
                               END;
                               ";

                connection.Execute(sql);
            }
        }

        private static void SeedStacks()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                               IF NOT EXISTS (
                                 SELECT 1 FROM Stacks
                               )
                               BEGIN
                                INSERT INTO Stacks (Name) VALUES
                                ('Spanish'),
                                ('German'),
                                ('Polish');
                               END;
                              ";

                connection.Execute(sql);
            }
        }

        private static void SeedFlashcards()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                               IF NOT EXISTS (
                                 SELECT 1 FROM Flashcards
                               )
                               BEGIN
                                INSERT INTO Flashcards (StackId, Front, Back) VALUES
                                (1, 'Hola', 'Hello'),
                                (1, '¿Cómo estás?', 'How are you?'),
                                (1, 'Gracias', 'Thank you'),
                                (2, 'Hallo', 'Hello'),
                                (2, 'Wie geht es dir?', 'How are you?'),
                                (2, 'Danke', 'Thank you'),
                                (3, 'Dzień dobry', 'Good morning'),
                                (3, 'Do widzenia', 'Goodbye'),
                                (3, 'Proszę', 'Please')
                               END;";

                connection.Execute(sql);
            }
        }
    }
}
