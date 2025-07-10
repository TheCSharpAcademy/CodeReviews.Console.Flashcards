using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.Database;
internal static class GeneralDBHelper
{
    internal static bool InitializeDatabase()
    {
        bool initializationSuccessful = false;

        try
        {
            CreateDatabase();
            CreateTables();

            initializationSuccessful = true;

        }
        catch (SqlException ex)
        {
            
            AnsiConsole.WriteLine($"\n\nAn error has occurred while initializing the database. Error message: {ex.Message}\n\n");
            initializationSuccessful = false;
        }

        return initializationSuccessful;
    }

    internal static SqlConnection CreateSqlConnection(string connectionString)
    {
        try
        {
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
        catch (SqlException ex)
        {
            AnsiConsole.WriteLine($"An error occurred creating the SQL Connection. Error message: {ex.ToString()}");
            throw;
        }
    }


    #region private methods

    private static void CreateDatabase()
    {
        string connectionString = ConfigurationManager.AppSettings.Get("masterConnectionString");
        using SqlConnection connection = CreateSqlConnection(connectionString);

        string sql = @"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Flashcards')
                                BEGIN
                                    CREATE DATABASE [Flashcards]
                                END
                               ";
        connection.Execute(sql);
    }

    private static void CreateTables()
    {
        string connectionString = ConfigurationManager.AppSettings.Get("flashcardsConnectionString");
        using SqlConnection connection = new SqlConnection(connectionString);
        string sql = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Decks' AND xtype='U')
                                BEGIN
                                    CREATE TABLE Decks (
                                        Id INT PRIMARY KEY IDENTITY (1,1),
                                        Name VARCHAR(255) NOT NULL
                                    );
                                END
                               IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Flashcards' AND xtype='U')
                                BEGIN
                                    CREATE TABLE Flashcards (
                                        Id INT PRIMARY KEY IDENTITY (1,1),
                                        Front NVARCHAR(255) NOT NULL,
                                        Back NVARCHAR(255) NOT NULL,
                                        DeckId INT NOT NULL,
                                        FOREIGN KEY (DeckId) REFERENCES Decks(Id)
                                    );
                                END
                               IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StudySessions' AND xtype='U')
                                BEGIN
                                    CREATE TABLE StudySessions (
                                        Id INT PRIMARY KEY IDENTITY (1,1),
                                        Score INT NOT NULL,
                                        CardsStudied INT NOT NULL,
                                        SessionDate DATETIME NOT NULL,
                                        DeckName VARCHAR(255),
                                        DeckId INT NOT NULL,
                                        FOREIGN KEY (DeckId) REFERENCES Decks(Id)
                                    );
                                END
                               ";
        connection.Execute(sql);
    }

    #endregion private methods
}
