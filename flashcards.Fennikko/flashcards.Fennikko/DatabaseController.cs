using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace flashcards.Fennikko;

public class DatabaseController
{
    public static readonly string InitialConnection = ConfigurationManager.AppSettings.Get("initialConnectionString");

    public static string ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");

    public static void DatabaseCreation()
    {
        using var initialConnection = new SqlConnection(InitialConnection);
        var testQuery = "SELECT database_id FROM sys.databases WHERE name = 'Flashcards'";
        var testDatabaseExists = initialConnection.Query(testQuery);
        if (testDatabaseExists.Any()) return;
        var databaseCreation = """
                               CREATE DATABASE Flashcards ON PRIMARY
                               (NAME = Flashcards,
                               FILENAME = 'C:\temp\Flashcards.mdf',
                               SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                               LOG ON (NAME = MyDatabase_Log,
                               FILENAME = 'C:\temp\FlashCardsLog.ldf',
                               SIZE = 1MB,
                               MAXSIZE = 5MB,
                               FILEGROWTH = 10%)
                               """;
        initialConnection.Execute(databaseCreation);
        using var connection = new SqlConnection(ConnectionString);
        connection.Execute(
            """
            IF OBJECT_ID(N'stacks', N'U') IS NULL
            CREATE TABLE stacks (
                StackId int IDENTITY(1,1) PRIMARY KEY,
                StackName VARCHAR(255) NOT NULL,
                UNIQUE (StackName)
                )
            """);
        connection.Execute(
            """
            IF OBJECT_ID(N'flash_cards', N'U') IS NULL
            CREATE TABLE flash_cards (
            FlashcardId int IDENTITY(1,1) PRIMARY KEY,
            FlashcardIndex int NOT NULL,
            CardFront VARCHAR(255) NOT NULL,
            CardBack VARCHAR(255) NOT NULL,
            StackId int NOT NULL,
            UNIQUE (CardFront),
            CONSTRAINT FK_flash_cards_stacks FOREIGN KEY (StackId)
            REFERENCES stacks (StackId)
            ON DELETE CASCADE
            )
            """);
        connection.Execute(
            """
            IF OBJECT_ID(N'study_sessions', N'U') IS NULL
            CREATE TABLE study_sessions (
            StudyId int IDENTITY(1,1) PRIMARY KEY,
            SessionDate DateTime NOT NULL,
            SessionScore int NOT NULL,
            StackId int NOT NULL,
            CONSTRAINT FK_study_sessions_stacks FOREIGN KEY (StackId)
            REFERENCES stacks (StackId)
            ON DELETE CASCADE
            )
            """);
    }
}