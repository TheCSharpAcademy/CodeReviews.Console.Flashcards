using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Data;

internal class FlashcardDelete
{
    internal static void DeleteFlashcard()
    {
        Console.Clear();
        var stack = StackRead.GetStack();
        if (stack.StackName == null) return;

        Delete(stack.StackId);
    }

    internal static void Delete(int stackId)
    {
        Console.Clear();
        var flashcardMap = FlashcardRead.MakeFlashcardMap(stackId);
        if (DisplayInfoHelpers.NoRecordsAvailable(flashcardMap.Keys)) return;

        var choice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Choose a flashcard to delete", flashcardMap.Keys);
        if (choice == DisplayInfoHelpers.Back)
        {
            Console.Clear();
            return;
        }

        var success = flashcardMap.TryGetValue(choice, out Flashcard flashcardToDelete);
        if (!success) return;

        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@FlashcardId", flashcardToDelete.FlashcardId, DbType.Int64);

            var exists = connection.QueryFirstOrDefault<bool>(@"
                SELECT CASE
                    WHEN EXISTS (SELECT 1 FROM Flashcard WHERE FlashcardId = @FlashcardId) THEN 1
                    ELSE 0
                END", parameters);

            if (exists)
            {
                AnsiConsole.MarkupLine($"[red]WARNING!\nYou want to delete that flashcard permanently![/]");
                AnsiConsole.MarkupLine($"{choice}\n");
                if (!DisplayInfoHelpers.ConfirmDeletion())
                {
                    Console.Clear();
                    return;
                }

                connection.Execute(
                    "DELETE FROM Flashcard WHERE FlashcardId = @FlashcardId",
                    parameters);

                AnsiConsole.MarkupLine("Flashcard deleted successfully.");
                DisplayInfoHelpers.PressAnyKeyToContinue();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Flashcard not found in the database.[/]");
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
