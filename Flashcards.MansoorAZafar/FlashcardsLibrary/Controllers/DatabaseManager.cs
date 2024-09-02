using Dapper;
using Microsoft.Data.SqlClient;
using FlashcardsLibrary.Models;
using System.Data;
using FlashcardsLibrary.Views;

namespace FlashcardsLibrary.Controllers;

internal class DatabaseManager
{
    private static string? ConnectionString;
    static DatabaseManager()
    {
        ConnectionString = Utilities.GetDatabaseConnectionString();
        if(ConnectionString == "n/a")
        {
            System.Console.WriteLine("Connection string is n/a, Please modify/check it again\n");
            DataViewer.Figlet("Goodbye");
            System.Environment.Exit(0);
        }
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();


        var tableCommand = connection.CreateCommand();
        tableCommand.CommandText = 
            @"IF NOT EXISTS (SELECT * FROM sys.tables where name = 'Stack')
            BEGIN
                CREATE TABLE Stack (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Topic NVARCHAR(255) UNIQUE
                )
            END";
        tableCommand.ExecuteNonQuery();


        tableCommand.CommandText = 
            @"IF NOT EXISTS (Select * FROM sys.tables WHERE name = 'Flashcard')
            BEGIN
                CREATE TABLE Flashcard (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    FrontOfTheCard NVARCHAR(255),
                    BackOfTheCard NVARCHAR(255),
                    Topic NVARCHAR(255),
                    FOREIGN KEY (Topic) REFERENCES Stack(Topic)
                )
            END";
        tableCommand.ExecuteNonQuery();    
        

        tableCommand.CommandText =
            @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'fix_flashcard_Ids')
            BEGIN
                EXEC('CREATE PROCEDURE fix_flashcard_Ids AS BEGIN SET NOCOUNT ON; END');
            END";
        tableCommand.ExecuteNonQuery();


        tableCommand.CommandText =
            @"ALTER PROCEDURE fix_flashcard_Ids
            AS
            BEGIN
                DECLARE @TempTable TABLE (Id INT, FrontOfTheCard NVARCHAR(255), BackOfTheCard NVARCHAR(255), Topic NVARCHAR(255));

                -- Insert data into the temporary table without the identity column
                INSERT INTO @TempTable (Id, FrontOfTheCard, BackOfTheCard, Topic)
                SELECT ROW_NUMBER() OVER (ORDER BY Id ASC), FrontOfTheCard, BackOfTheCard, Topic
                FROM Flashcard;

                -- Delete all rows from the Flashcard table
                DELETE FROM Flashcard;
                DBCC CHECKIDENT ('Flashcard', RESEED, 0);

                -- Insert the reordered data back into the Flashcard table
                INSERT INTO Flashcard (FrontOfTheCard, BackOfTheCard, Topic)
                SELECT FrontOfTheCard, BackOfTheCard, Topic FROM @TempTable;
            END";
        tableCommand.ExecuteNonQuery();
        connection.Close();
    }

    //tableName will always be managed by a utilities static string never user input
    internal List<T> GetAllData<T>(string tableName)
    {
        List<T> data = new();
        
        using var connection = new SqlConnection(ConnectionString);
        data = connection.Query<T>($"SELECT * FROM {tableName} ORDER BY Id ASC").ToList();
        
        connection.Close();
        return data;
    }

    //tableName will always be managed by a utilities static string never user input
    internal List<T> GetAllDataWithTopic<T>(string tableName, string topic)
    {
        List<T> data = new();

        using var connection = new SqlConnection(ConnectionString);
        
        string sql = $"SELECT * FROM {tableName} WHERE Topic = @Topic";
        data = connection.Query<T>(sql, new {Topic =  topic}).ToList();
        
        connection.Close();
        return data;
    }

    internal int CreateFlashcardInStack(string front, string back, string topic)
    {
        int rowsModified = 0;

        using var connection = new SqlConnection(ConnectionString);
        var sql = 
            @"INSERT INTO Flashcard(FrontOfTheCard, BackOfTheCard, Topic)
              Values(@FrontOfTheCard, @BackOfTheCard, @Topic)";
        
        Flashcard flashcard = new Flashcard(front, back, topic);
        rowsModified = connection.Execute(sql, flashcard);
        
        connection.Close();
        return rowsModified;
    }

    internal void CreateStack(string topic)
    {
        using var connection = new SqlConnection(ConnectionString);
        var sql = @"INSERT INTO Stack(Topic)
                    VALUES(@Topic)";
        Stack stack = new(topic);
        
        connection.Execute(sql, stack);
        connection.Close();
    }

    internal void DeleteFlashcard(int id)
    {
        using var connection = new SqlConnection(ConnectionString);
        Stack toBeDeleted = new(id); 
        
        connection.Execute("DELETE FROM Flashcard WHERE Id=@Id", toBeDeleted);
        connection.Execute("EXEC fix_flashcard_Ids");
        connection.Close();
    }

    internal void UpdateFlashCard(int id, string front, string back, string topic)
    {
        using var connection = new SqlConnection(ConnectionString);

        Flashcard toBeUpdated = new(id, front, back, topic);
        var sql = 
            @"UPDATE Flashcard
                SET FrontOfTheCard = @FrontOfTheCard,
                    BackOfTheCard = @BackOfTheCard,
                    Topic = @Topic
                WHERE
                    Id = @Id";

        connection.Execute(sql, toBeUpdated);
        connection.Close();
    }

    internal bool TopicFree(string topic)
    {  
        int queryLength = 0;
        using var connection = new SqlConnection(ConnectionString);
        
        Stack toBeSearched = new(topic);
        queryLength = connection.ExecuteScalar<int>("SELECT * FROM Stack WHERE Topic = @Topic", toBeSearched);

        connection.Close();
        return queryLength == 0;
    }

    internal bool IDExistsStack(int id)
    {
        int queryLength = 0;
        using var connection = new SqlConnection(ConnectionString);

        Stack toBeSearched = new(id);
        queryLength = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Stack WHERE Id = @Id", toBeSearched);

        connection.Close();
        return queryLength > 0;
    }

    internal bool IDExistsFlashcard(int id)
    {
        int queryLength = 0;
        using var connection = new SqlConnection(ConnectionString);

        Flashcard toBeSearched = new(id);
        queryLength = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Flashcard WHERE Id = @Id", toBeSearched);

        connection.Close();
        return queryLength > 0;
    }

    internal int GetNumberOfEntriesFromStack()
    {
        int queryLength = 0;
        using var connection = new SqlConnection(ConnectionString);

        queryLength = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Stack");

        connection.Close();
        return queryLength;
    }

    internal int GetNumberOfEntriesFromFlashcards()
    {
        int queryLength = 0;
        using var connection = new SqlConnection(ConnectionString);

        queryLength = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Flashcard");

        connection.Close();
        return queryLength;
    }

    internal static int StaticStackExists(string ConnectionString)
    {
        int queryLength = 0;
        using var connection = new SqlConnection(ConnectionString);

        string sql = 
            @"IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Stack')
            BEGIN
                SELECT COUNT(*) FROM Stack
            END
            ELSE
            BEGIN
                SELECT 0
            END";
        queryLength = connection.ExecuteScalar<int>(sql);

        connection.Close();
        return queryLength;
    }

    internal string GetTopicFromStackID(int id)
    {
        string topic;
        using var connection = new SqlConnection(ConnectionString);

        Stack toBeFound = new(id);
        topic = connection.QuerySingleOrDefault<string>("SELECT Topic FROM Stack WHERE Id = @Id", toBeFound);
        
        connection.Close();

        return topic ?? "";
    }

    internal void DeleteStack(int id, string topic)
    {
        Stack toBeDelted = new(id, topic);
        using var connection = new SqlConnection(ConnectionString);
        
        string deleteSQL = 
            @"DELETE FROM Flashcard WHERE Topic = @Topic;
              DELETE FROM Stack WHERE Id = @Id;";
        connection.Execute(deleteSQL, toBeDelted);

        connection.Close();
    }
}
