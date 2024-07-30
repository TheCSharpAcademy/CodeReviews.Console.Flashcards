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

    static void RunQuery(string query)
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

        // string query = "SELECT Start, End, Duration FROM Sessions";
        // List<CodingSession> sessions = connection.Query<CodingSession>(query).ToList();
        // CodingSession.Sessions = sessions;
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
        INSERT INTO Sessions (Start, End, Duration)
        VALUES (@Start, @End, @Duration)";
        connection.Execute(query, stack);
        connection.Close();
    } // end of InsertStack Method
}