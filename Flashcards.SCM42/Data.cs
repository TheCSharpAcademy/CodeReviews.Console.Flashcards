using System.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Flashcards;

public class DataAccess
{
    static string connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;
    static string masterConnectionString = ConfigurationManager.ConnectionStrings["Master"].ConnectionString;
    
    internal static void CreateDatabase()
    {
        var sqlCommand = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FlashcardsDB') CREATE DATABASE FlashcardsDB";

        using (var connection = new SqlConnection(masterConnectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sqlCommand);
                Console.WriteLine("Database was created.");
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }         
        }
    }

    internal static void CreateTables()
    {
        var createStackTable = @"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
                                BEGIN
                                    CREATE TABLE Stacks (
                                        StackId INT IDENTITY(1,1) PRIMARY KEY,
                                        StackName NVARCHAR(50) NOT NULL UNIQUE ,
                                        CardQuantity INT
                                    );
                                END";

        var createFlashcardTable = @"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Flashcards')
                                    BEGIN
                                        CREATE TABLE Flashcards (
                                        CardId INT IDENTITY(1,1) PRIMARY KEY,
                                        Front NVARCHAR(50) NOT NULL UNIQUE,
                                        Back NVARCHAR(50) NOT NULL,
                                        StackId INT NOT NULL,
                                        CONSTRAINT FK_StackId
                                            FOREIGN KEY (StackId) REFERENCES Stacks(StackId)
                                            ON DELETE CASCADE
                                        );
                                    END";
        
        var createSessionTable = @"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Sessions')
                                BEGIN
                                    CREATE TABLE Sessions (
                                        SessionId INT IDENTITY(1,1) PRIMARY KEY,
                                        Date DATE NOT NULL,
                                        FlashcardsShown INT NULL,
                                        StackName NVARCHAR(50) NOT NULL,
                                        Points INT NOT NULL,
                                        StackId INT NOT NULL,
                                        CONSTRAINT FK_SessionStackId
                                            FOREIGN KEY (StackId) REFERENCES Stacks(StackId)
                                            ON DELETE CASCADE
                                    );
                                END";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(createStackTable);
                connection.Execute(createFlashcardTable);
                connection.Execute(createSessionTable);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }
    }
}