using Spectre.Console;
using System.Data;
using Dapper;
using System.Data.SqlClient;


namespace Flashcards.DatabaseUtilities;

internal class DatabaseHelper
{
    private static void DatabaseConnection(out IDbConnection? dbConnection)
    {
        try
        {
            IDbConnection connection = new SqlConnection(Program.ConnectionString);
            dbConnection = connection;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]An error has occured: {ex.Message} while connecting to the database[/]");
            dbConnection = null;
        }
    }

    public static void RunQuery(string query)
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;

        connection.Open();
        connection.Execute(query);
        connection.Close();
    } // end of RunQuery Method

    public static void GetStacks()
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = "SELECT StackName, StackSize, Id FROM Stacks";
        CardStack.Stacks = connection.Query<CardStack>(query).ToList();
        query = "SELECT Front, Back, Stack_Id FROM Cards";
        List<Card> cards = connection.Query<Card>(query).ToList();

        foreach(CardStack stack in CardStack.Stacks)
            foreach(Card card in cards)
                if (card.StackId == stack.Id)
                    stack.Cards.Add(card);

        connection.Close();
    } // end of GetStack Method

    public static void InsertStack(CardStack stack)
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = @"
        INSERT INTO dbo.Stacks (StackName, StackSize)
        VALUES (@StackName, @StackSize);
        SELECT CAST(SCOPE_IDENTITY() as INT);";
        int stackID = connection.QuerySingle<int>(query, stack);

        query = @"
        INSERT INTO dbo.Cards (Front, Back, Stack_ID)
        VALUES (@Front, @Back, @Stack_ID)";
        foreach(Card card in stack.Cards)
        {
            var parameters = new { Front = card.Front, Back = card.Back, Stack_ID = stackID };
            connection.Execute(query, parameters);
        }
        connection.Close();
    } // end of InsertStack Method

    public static void GetSessions()
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = "SELECT StackName, StackSize, Stack_Id, NumComplete, NumCorrect, AvgTime, Date FROM Sessions";
        StudySession.Sessions = connection.Query<StudySession>(query).ToList();

        connection.Close();
    } // end of GetStack Method

    public static void InsertSession(StudySession session)
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        int stackID = connection.QuerySingle<int>($"SELECT Id FROM dbo.Stacks WHERE StackName = '{session.StackName}'", session);

        string query = @$"
        INSERT INTO dbo.Sessions (StackName, StackSize, Stack_Id, NumComplete, NumCorrect, AvgTime, Date)
        VALUES (@StackName, @StackSize, {stackID}, @NumComplete, @NumCorrect, @AvgTime, @Date);";
        connection.Execute(query, session);

        connection.Close();
    } // end of InsertStack Method

    public static void GetRowid(int Id, int StackId)
    {
        string query = $@"SELECT row_id";
        RunQuery(query);
    }

    public static void InitializeDatabase()
    {
        string query = $@"
USE master; IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'FlashcardDB')
    CREATE DATABASE [FlashcardDB];
GO

USE FlashcardDB; 
GO

IF OBJECT_ID(N'dbo.Stacks', N'U') IS NULL
CREATE TABLE dbo.Stacks (
Id INT NOT NULL PRIMARY KEY IDENTITY,
StackName NVARCHAR(255) NOT NULL,
StackSize INT NOT NULL
);

IF OBJECT_ID(N'dbo.Cards', N'U') IS NULL
CREATE TABLE dbo.Cards (
Id INT NOT NULL PRIMARY KEY IDENTITY,
Front NVARCHAR(255) NOT NULL,
Back NVARCHAR(255) NOT NULL,
Stack_Id INT NOT NULL,
FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id)
);

IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
CREATE TABLE dbo.Sessions (
Id INT NOT NULL PRIMARY KEY IDENTITY,
StackName NVARCHAR(255) NOT NULL,
StackSize INT NOT NULL,
Stack_Id INT NOT NULL,
NumComplete INT NOT NULL,
NumCorrect INT NOT NULL,
AvgTime FLOAT NOT NULL,
Date NVARCHAR(255) NOT NULL,
FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id)
);
GO";

        RunQuery(query);
    }
}