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
        try
        {
            connection.Execute(query);
        }
        catch(Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]An error has occured while running a query: {ex.Message}[/]");
        }
        connection.Close();
    } // end of RunQuery Method

    public static void GetStacks()
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        try
        {
            CardStack.Stacks = connection.Query<CardStack>("SELECT StackName, Id FROM Stacks").ToList();
            List<Card> cards = connection.Query<Card>("SELECT Front, Back, Stack_Id FROM Cards").ToList();
            foreach(CardStack stack in CardStack.Stacks)
                foreach(Card card in cards)
                    if (card.StackId == stack.Id)
                        stack.Cards.Add(card);
        }
        catch(Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]An error has occured while getting the stacks from the database: {ex.Message}[/]");
        }

        connection.Close();
    } // end of GetStack Method

    public static void InsertStack(CardStack stack)
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = @"
        INSERT INTO dbo.Stacks (StackName)
        VALUES (@StackName);
        SELECT CAST(SCOPE_IDENTITY() as INT);";
        int stackID = connection.QuerySingle<int>(query, stack);

        query = @"
        INSERT INTO dbo.Cards (Front, Back, Stack_ID)
        VALUES (@Front, @Back, @Stack_ID)";
        try
        {
            foreach(Card card in stack.Cards)
            {
                var parameters = new { card.Front, card.Back, Stack_ID = stackID };
                connection.Execute(query, parameters);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]An error has occured while saving the stack: {ex.Message}[/]");
        }
        connection.Close();
    } // end of InsertStack Method

    public static void InsertCard(Card card, int stackID)
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = $@"
        INSERT INTO dbo.Cards (Front, Back, Stack_ID)
        VALUES (@Front, @Back, '{stackID}')";

        try
        {
            connection.Execute(query, card);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]An error has occured while saving the card: {ex.Message}[/]");
        }
        connection.Close();
    } // end of InsertStack Method

    public static void GetSessions()
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = "SELECT Date, NumComplete, NumCorrect, StackName, Stack_Id, AvgTime FROM Sessions";
        try
        {
            StudySession.Sessions = connection.Query<StudySession>(query).ToList();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]An error has occured while getting the sessions from the database: {ex.Message}[/]");
        }

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
        INSERT INTO dbo.Sessions (Date, NumComplete, NumCorrect, AvgTime, StackName, Stack_Id)
        VALUES (@Date, @NumComplete, @NumCorrect, @AvgTime, @StackName, {stackID});";
        try
        {
            connection.Execute(query, session);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]An error has occured while saving the session: {ex.Message}[/]");
        }

        connection.Close();
    } // end of InsertStack Method

    public static void InitializeDatabase()
    {
        string query = @"
USE master
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'FlashcardDB')
    CREATE DATABASE [FlashcardDB]";
    RunQuery(query);
    RunQuery("USE FlashcardDB");
        query = @"
IF OBJECT_ID(N'dbo.Stacks', N'U') IS NULL
CREATE TABLE dbo.Stacks (
    Id INT NOT NULL PRIMARY KEY IDENTITY,
    StackName NVARCHAR(255) NOT NULL
)

IF OBJECT_ID(N'dbo.Cards', N'U') IS NULL
CREATE TABLE dbo.Cards (
    Id INT NOT NULL PRIMARY KEY IDENTITY,
    Front NVARCHAR(255) NOT NULL,
    Back NVARCHAR(255) NOT NULL,
    Stack_Id INT NOT NULL,
    FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id)
)

IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
CREATE TABLE dbo.Sessions (
    Id INT NOT NULL PRIMARY KEY IDENTITY,
    Date NVARCHAR(255) NOT NULL,
    NumComplete INT NOT NULL,
    NumCorrect INT NOT NULL,
    AvgTime FLOAT NOT NULL,
    StackName NVARCHAR(255) NOT NULL,
    Stack_Id INT NOT NULL,
    FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id)
)";

        RunQuery(query);
    }
}