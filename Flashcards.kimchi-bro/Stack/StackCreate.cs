using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

internal class StackCreate
{
    internal static void Create()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Creating a new stack of flashcards.[/]\n");
        var stackName = InputHelpers.StringLengthCheck(50, "Enter a stack name (0 for exit):");
        if (stackName == "0")
        {
            Console.Clear();
            return;
        }

        AddNewStack(stackName);
    }

    private static void AddNewStack(string stackName)
    {
        using var connection = new SqlConnection(Config.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();

        while (true)
        {
            try
            {
                parameters.Add("@StackName", stackName);
                var existingStackId = connection.QueryFirstOrDefault<int>(@"
                SELECT StackId
                FROM Stack
                WHERE StackName = @StackName",
                    parameters);

                if (existingStackId != 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]A stack with the name '{stackName}' already exists.[/]");
                    stackName = InputHelpers.StringLengthCheck(50, "Enter a stack name (0 for exit):");
                    if (stackName == "0")
                    {
                        Console.Clear();
                        return;
                    }
                }
                else
                {
                    connection.Execute(@"
                    INSERT INTO Stack (StackName)
                    VALUES (@StackName)",
                        parameters);
                    AnsiConsole.MarkupLine($"[green]A new '{stackName}' stack created successfully![/]");
                    DisplayInfoHelpers.PressAnyKeyToContinue();
                    return;
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
}
