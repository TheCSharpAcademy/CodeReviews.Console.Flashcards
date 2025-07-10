using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.Database;
internal static class GeneralDBHelper
{
    private static readonly string _masterServerName = ConfigurationManager.AppSettings.Get("masterServer");
    private static readonly string _masterDatabaseName = ConfigurationManager.AppSettings.Get("masterDatabase");
    private static readonly string _masterTrustedConnection = ConfigurationManager.AppSettings.Get("masterTrustedConnection");
    private static readonly string _serverName = ConfigurationManager.AppSettings.Get("server");
    private static readonly string _databaseName = ConfigurationManager.AppSettings.Get("database");
    private static readonly string _trustedConnection = ConfigurationManager.AppSettings.Get("trustedConnection");
    
    private static readonly string _masterConnectionString = $"Server={_masterServerName};Database={_masterDatabaseName};Trusted_Connection={_masterTrustedConnection};";
    
    internal static readonly string ConnectionString = $"Server={_serverName};Database={_databaseName};Trusted_Connection={_trustedConnection};";

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
        using SqlConnection connection = CreateSqlConnection(_masterConnectionString);

        string sql = @$"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{_databaseName}')
                                BEGIN
                                    CREATE DATABASE [{_databaseName}] 
                                END
                               ";
        connection.Execute(sql);
    }

    private static void CreateTables()
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
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
