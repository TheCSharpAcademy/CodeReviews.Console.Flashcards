using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Data;

internal class StackUpdate
{
    internal static void Update()
    {
        Console.Clear();
        var stack = StackRead.GetStack();
        if (stack.StackName == null) return;

        Console.Clear();
        AnsiConsole.MarkupLine($"Updating [yellow]{stack.StackName}[/] stack\n");

        var actions = new Dictionary<string, Action>
        {
            { DisplayInfoHelpers.Back, Console.Clear },
            { "Add a new flashcard", () => FlashcardCreate.FlashcardAddingLoop(stack) },
            { "Update flashcard", FlashcardUpdate.UpdateFlashcard },
            { "Delete flashcard", () => FlashcardDelete.Delete(stack.StackId) },
            { "Rename stack", () => RenameStack(stack) }
        };

        var flashcardAction = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an action: ")
            .PageSize(10)
            .AddChoices(actions.Keys));

        actions[flashcardAction]();
    }

    private static void RenameStack(Stack stack)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"Renaming [yellow]{stack.StackName}[/] stack:");

        var stackName = InputHelpers.StringLengthCheck(50, "Enter new name:");

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
                var updParameters = new DynamicParameters();
                updParameters.Add("@StackName", stackName);
                updParameters.Add("@StackId", stack.StackId, DbType.Int64);
                connection.Execute(@"
                    UPDATE Stack
                    SET StackName = @StackName
                    WHERE StackId = @StackId", updParameters);

                AnsiConsole.MarkupLine(
                    $"Stack [yellow]{stack.StackName}[/] renamed successfully to [green]{stackName}[/].");
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
