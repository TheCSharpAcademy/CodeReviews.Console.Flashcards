using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Data;

internal class StackDelete
{
    internal static void Delete()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[red]Deleting stack.[/]\n");
        var stack = StackRead.GetStack();
        if (stack.StackName == null) return;

        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", stack.StackId, DbType.Int64);
            var exists = connection.QueryFirstOrDefault<bool>(@"
                SELECT CASE
                    WHEN EXISTS (SELECT 1 FROM Stack WHERE StackId = @StackId) THEN 1
                    ELSE 0
                END", parameters);

            if (exists)
            {
                AnsiConsole.MarkupLine($"[red]WARNING!!![/] You want to delete that stack permanently! => [red]{stack.StackName}[/]");
                AnsiConsole.MarkupLine($"[red]WARNING!!![/] All flashcards it contains will be deleted!");
                AnsiConsole.MarkupLine($"[red]WARNING!!![/] And all study sessions with that stack will be also deleted!\n");
                AnsiConsole.MarkupLine($"[red]That operation can not be reversed![/]");
                if (!DisplayInfoHelpers.ConfirmDeletion())
                {
                    Console.Clear();
                    return;
                }

                connection.Execute(
                    "DELETE FROM Stack WHERE StackId = @StackId", parameters);

                AnsiConsole.MarkupLine($"Stack [red]{stack.StackName}[/] deleted successfully.");
                DisplayInfoHelpers.PressAnyKeyToContinue();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Stack not found in the database.[/]");
                DisplayInfoHelpers.PressAnyKeyToContinue();
            }
        }
        catch (SqlException ex)
        {
            DisplayErrorHelpers.SqlError(ex);
        }
        catch (Exception ex)
        {
            DisplayErrorHelpers.GeneralError(ex);
        }
    }
}
