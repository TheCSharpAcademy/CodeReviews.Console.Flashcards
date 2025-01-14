using Dapper;
using Microsoft.Data.SqlClient;

internal class Database
{
    public static void Initialize()
    {
        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            connection.Execute(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stack')
                    CREATE TABLE Stack (
                        StackId INT IDENTITY(1,1) PRIMARY KEY,
                        StackName NVARCHAR(50) NOT NULL UNIQUE
                    );
            
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcard')
                    CREATE TABLE Flashcard (
                        FlashcardId INT IDENTITY(1,1) PRIMARY KEY,
                        StackId INT NOT NULL,
                        StackName NVARCHAR(50),
                        FlashcardFront NVARCHAR(200),
                        FlashcardBack NVARCHAR(200)
                    );

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Session')
                    CREATE TABLE Session (
                        SessionId INT IDENTITY(1,1) PRIMARY KEY,
                        StackId INT NOT NULL,
                        StackName NVARCHAR(50),
                        SessionDate DATETIME,
                        SessionScore INT
                    );

                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Flashcard_Stack')
                    ALTER TABLE Flashcard
                    ADD CONSTRAINT FK_Flashcard_Stack
                    FOREIGN KEY (StackName)
                    REFERENCES Stack(StackName)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE;

                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Session_Stack')
                    ALTER TABLE Session
                    ADD CONSTRAINT FK_Session_Stack
                    FOREIGN KEY (StackName)
                    REFERENCES Stack(StackName)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE;");
        }
        catch (SqlException ex)
        {
            DisplayErrorHelpers.SqlError(ex);
        }
        catch (Exception ex)
        {
            DisplayErrorHelpers.GeneralError(ex);
        }
    }
}
