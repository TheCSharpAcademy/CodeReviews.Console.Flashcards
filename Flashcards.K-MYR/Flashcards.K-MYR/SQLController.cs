using System.Configuration;
using System.Data.SqlClient;
using Flashcards.K_MYR.Models;

namespace Flashcards.K_MYR;


internal class SQLController
{
    static private readonly string connectionString = @$"Data Source= {ConfigurationManager.AppSettings.Get("sqlServerName")}; Initial Catalog = {ConfigurationManager.AppSettings.Get("dbName")}";

    internal static void CreateDbIfNotExists()
    {
        using SqlConnection connection = new(@$"Data Source={ConfigurationManager.AppSettings.Get("sqlServerName")}");
        connection.Open();
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TEST')
                            CREATE DATABASE TEST;";
        cmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static void CreateTablesIfNotExists()
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        SqlCommand cmd = connection.CreateCommand();

        cmd.CommandText = @"IF OBJECT_ID(N'dbo.Stacks', N'U') IS NULL
                            CREATE TABLE dbo.Stacks (
                                                     StackId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
                                                     Name NVARCHAR(25) NOT NULL UNIQUE,
                                                     NumberOfCards INT DEFAULT 0,
                                                     Created DATETIME NOT NULL DEFAULT GETDATE()
                                                    );    

                             IF OBJECT_ID(N'dbo.Flashcards', N'U') IS NULL 
                             BEGIN
                             CREATE TABLE dbo.Flashcards (
                                                          FlashcardId INT IDENTITY(1,1)PRIMARY KEY NOT NULL,
                                                          StackId INT NOT NULL,
                                                          FrontText VARCHAR(50) NOT NULL,
                                                          BackText VARCHAR(50) NOT NULL,
                                                          Created DATETIME NOT NULL DEFAULT GETDATE()
                                                         ); 
                             ALTER TABLE dbo.Flashcards WITH Check ADD CONSTRAINT FK_Stacks_Flashcards 
                             FOREIGN KEY (StackId) REFERENCES Stacks(StackId)                             
                             END      

                             IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
                             BEGIN
                             CREATE TABLE dbo.Sessions (
                                                        SessionId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
                                                        StackId INT NOT NULL,
                                                        Date DATETIME NOT NULL DEFAULT GETDATE(),
                                                        Score INT NOT NULL,
                                                       );                           
                             ALTER TABLE dbo.Sessions WITH Check ADD CONSTRAINT FK_Stacks_Sessions 
                             FOREIGN KEY (StackId) REFERENCES Stacks(StackId);
                             END";

        cmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static List<CardStack> SelectStacksFromDB(string columns = "*", string args = "")
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();
        command.CommandText = $@"SELECT {columns} FROM Stacks {args}";

        SqlDataReader reader = command.ExecuteReader();

        List<CardStack> stacks = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                stacks.Add(new CardStack
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    NumberOfCards = reader.GetInt32(2),
                    CreatedDate = reader.GetDateTime(3),
                });
            }
        }

        return stacks;
    }

    internal static int StackNameExists(string name)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"SELECT 1 FROM dbo.Stacks WHERE Name = '{name}'"; 

        return Convert.ToInt32(command.ExecuteScalar());
    }

    internal static void DeleteStack(int iD)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"DELETE FROM dbo.Stacks WHERE StackId = {iD}";
        command.ExecuteNonQuery();
        connection.Close();

    }

    internal static void InsertStack(string name)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"INSERT INTO dbo.Stacks (Name) VALUES ('{name}')";
        command.ExecuteNonQuery();
        connection.Close();
    }

    internal static void UpdateStack(string name, int iD)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        SqlCommand command = connection.CreateCommand();

        command.CommandText = $"UPDATE dbo.Stacks SET Name = '{name}' WHERE StackId = {iD}";
        command.ExecuteNonQuery();
        connection.Close();
    }
}
