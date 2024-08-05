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

        AnsiConsole.MarkupLine("[yellow]RunQuery is incomplete[/]");
        OutputUtilities.ReturnToMenu("");
    } // end of RunQuery Method

    public static void GetStack()
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = "SELECT StackName, StackSize, Duration FROM Sessions";
        CardStack.Stacks = connection.Query<CardStack>(query).ToList();
        foreach(CardStack stack in CardStack.Stacks)
        {
            // fill each stacks card array with the cards that match its id
        }
        connection.Close();
        AnsiConsole.MarkupLine("[yellow]GetStack is incomplete[/]");
        OutputUtilities.ReturnToMenu("");
    } // end of GetStack Method

    public static void GetCards()
    {
        AnsiConsole.MarkupLine("[yellow]GetCards is incomplete[/]");
        OutputUtilities.ReturnToMenu("");
    } // end of GetCards Method

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
            var parameters = new { Front = card.front, Back = card.back, Stack_ID = stackID };
            connection.Execute(query, parameters);
        }
        connection.Close();
    } // end of InsertStack Method
}