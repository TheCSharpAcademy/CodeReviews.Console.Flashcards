using Microsoft.Data.SqlClient;

namespace Flashcards.jkjones98;

internal class StackCreator
{
    internal void CreateStackTable(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = 
            @"USE FlashcardsDb
            IF (NOT EXISTS (SELECT * 
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'dbo'
            AND TABLE_NAME = 'Stacks'))
            BEGIN
                CREATE TABLE Stacks (
                    StackId INTEGER IDENTITY(1,1) PRIMARY KEY,
                    StackName TEXT)
            END;";
        tableCmd.ExecuteNonQuery();
    }

    internal void CreateFlashcardTable(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText =  
            @"USE FlashcardsDb
            IF (NOT EXISTS (SELECT * 
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'dbo'
            AND TABLE_NAME = 'Flashcards'))
            BEGIN
                CREATE TABLE Flashcards (
                    FlashId INTEGER IDENTITY(1,1) PRIMARY KEY,
                    Front TEXT,
                    Back TEXT,
                    StackId INTEGER FOREIGN KEY REFERENCES Stacks(StackId)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE)
            END;";
        tableCmd.ExecuteNonQuery();
    }

    internal void CreateStudySessionTable(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText =  
            @"USE FlashcardsDb
            IF (NOT EXISTS (SELECT * 
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'dbo'
            AND TABLE_NAME = 'StudySessions'))
            BEGIN
                CREATE TABLE StudySessions (
                    StudyId INTEGER IDENTITY(1,1) PRIMARY KEY,
                    Date TEXT,
                    Score INTEGER,
                    Studied INTEGER,
                    Language TEXT,
                    StackId INTEGER FOREIGN KEY REFERENCES Stacks(StackId)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE)
            END;";
        tableCmd.ExecuteNonQuery();
    }
}