using Microsoft.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace Flashcards.Tables;

public class DatabaseSetup
{
    private static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");
    public static void EnsureDatabaseSetup()
    {
        using var connection = new SqlConnection(connectionString);
        
        string createStacksTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stacks' AND xtype='U')
            CREATE TABLE Stacks (
                ID INT PRIMARY KEY IDENTITY(1, 1),
                NAME VARCHAR(50) UNIQUE NOT NULL
            )";

        connection.Execute(createStacksTable);

        string createFlashcardsTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Flashcards' AND xtype='U')
            CREATE TABLE Flashcards (
                ID INT PRIMARY KEY IDENTITY(1,1),
                Question TEXT NOT NULL,
                Answer TEXT NOT NULL,
                DisplayID INT NOT NULL,
                StackID INT NOT NULL FOREIGN KEY REFERENCES Stacks(ID) ON DELETE CASCADE
            )";
            
        connection.Execute(createFlashcardsTable);
    
        string createStudySessionsTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StudySessions' and xtype='U')
            CREATE TABLE StudySessions (
                ID INT PRIMARY KEY IDENTITY(1, 1),
                Date DATETIME NOT NULL,
                Score INT NOT NULL,
                StackID INT NOT NULL FOREIGN KEY REFERENCES Stacks(ID) ON DELETE CASCADE
        )";

        connection.Execute(createStudySessionsTable);
    }
}
