using System.Data.SqlClient;
using Spectre.Console;

namespace Flashcards.empty_codes.Data;

internal static class Database
{
    public static string ConnectionString;

    public static void VerifyDatabaseAndTables()
    {
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();

        string dbName = new SqlConnectionStringBuilder(ConnectionString).InitialCatalog;
        string checkDatabaseQuery = $"SELECT database_id FROM sys.databases WHERE name = '{dbName}'";

        using var checkDbCommand = new SqlCommand(checkDatabaseQuery, conn);
        var dbExists = checkDbCommand.ExecuteScalar() != null;

        if (!dbExists)
        {
            AnsiConsole.MarkupLine($"[red]Database '{dbName}' does not exist.[/]");
            return;
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]Database '{dbName}' exists.[/]");
        }

        string[] tableNames = { "Stacks", "Flashcards", "StudySessions" };

        foreach (var tableName in tableNames)
        {
            string checkTableQuery = $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";

            using var checkTableCommand = new SqlCommand(checkTableQuery, conn);
            var tableExists = checkTableCommand.ExecuteScalar() != null;

            if (tableExists)
            {
                AnsiConsole.MarkupLine($"[green]Table '{tableName}' exists.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Table '{tableName}' does not exist.[/]");
            }
        }
        AnsiConsole.WriteLine("Press any key to continue.");
        Console.ReadKey();
        Console.Clear();
    }
}
