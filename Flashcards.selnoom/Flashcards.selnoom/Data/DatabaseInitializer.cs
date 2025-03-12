using Microsoft.Data.SqlClient;

namespace Flashcards.selnoom.Data;

internal static class DatabaseInitializer
{
    private static readonly string masterConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

    private static readonly string flashcardDbConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=FlashcardDB;Trusted_Connection=True;";

    public static void InitializeDatabase()
    {
        EnsureDatabaseExists();
        CreateTables();
    }

    private static void EnsureDatabaseExists()
    {
        using (var connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();
            string checkDbQuery = @"
                    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FlashcardDB')
                    BEGIN
                        CREATE DATABASE FlashcardDB;
                    END";
            using (var command = new SqlCommand(checkDbQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
    private static void CreateTables()
    {
        using (var connection = new SqlConnection(flashcardDbConnectionString))
        {
            connection.Open();

            string createStackTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stack')
                    BEGIN
                        CREATE TABLE Stack (
                            StackId INT IDENTITY(1,1) PRIMARY KEY,
                            StackName NVARCHAR(100) NOT NULL
                        );
                    END";
            using (var command = new SqlCommand(createStackTable, connection))
            {
                command.ExecuteNonQuery();
            }

            string createFlashcardTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcard')
                    BEGIN
                        CREATE TABLE Flashcard (
                            FlashcardId INT IDENTITY(1,1) PRIMARY KEY,
                            StackId INT NOT NULL,
                            Question NVARCHAR(MAX) NOT NULL,
                            Answer NVARCHAR(MAX) NOT NULL,
                            CONSTRAINT FK_Flashcard_Stack FOREIGN KEY (StackId)
                                REFERENCES Stack(StackId) ON DELETE CASCADE
                        );
                    END";
            using (var command = new SqlCommand(createFlashcardTable, connection))
            {
                command.ExecuteNonQuery();
            }

            string createStudySessionTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySession')
                    BEGIN
                        CREATE TABLE StudySession (
                            StudySessionId INT IDENTITY(1,1) PRIMARY KEY,
                            StackId INT NOT NULL,
                            Score INT NOT NULL,
                            SessionDate DATETIME NOT NULL DEFAULT GETDATE(),
                            CONSTRAINT FK_StudySession_Stack FOREIGN KEY (StackId)
                                REFERENCES Stack(StackId) ON DELETE CASCADE
                        );
                    END";
            using (var command = new SqlCommand(createStudySessionTable, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
