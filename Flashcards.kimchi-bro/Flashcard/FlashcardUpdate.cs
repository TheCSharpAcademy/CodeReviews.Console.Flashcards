using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Data;

internal class FlashcardUpdate
{
    internal static void UpdateFlashcard()
    {
        Console.Clear();
        var stack = StackRead.GetStack();
        if (stack.StackName == null) return;

        ChooseFlashcardFromStackAndUpdate(stack.StackId);
    }

    private static void ChooseFlashcardFromStackAndUpdate(int stackId)
    {
        Console.Clear();
        var flashcardMap = FlashcardRead.MakeFlashcardMap(stackId);
        if (DisplayInfoHelpers.NoRecordsAvailable(flashcardMap.Keys)) return;

        var choice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Choose a flashcard to update", flashcardMap.Keys);
        if (choice == DisplayInfoHelpers.Back)
        {
            Console.Clear();
            return;
        }

        var success = flashcardMap.TryGetValue(choice, out Flashcard flashcardToUpdate);
        if (!success) return;

        var front = flashcardToUpdate.FlashcardFront;
        var back = flashcardToUpdate.FlashcardBack;

        AnsiConsole.MarkupLine($"Selected card: [yellow]{front}[/] => [yellow]{back}[/]");
        GetNewFlashcardInputDataAndUpdate(flashcardToUpdate);
    }

    private static void GetNewFlashcardInputDataAndUpdate(Flashcard flashcard)
    {
        string front = AnsiConsole.Ask<string>("Enter new flashcard's front:");
        string back = AnsiConsole.Ask<string>("Enter new flashcard's back:");

        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@FlashcardId", flashcard.FlashcardId, DbType.Int64);
            var exists = connection.QueryFirstOrDefault<bool>(@"
                SELECT CASE
                    WHEN EXISTS (SELECT 1 FROM Flashcard WHERE FlashcardId = @FlashcardId) THEN 1
                    ELSE 0
                END", parameters);

            if (exists)
            {
                var updParameters = new DynamicParameters();
                updParameters.Add("@FlashcardId", flashcard.FlashcardId, DbType.Int64);
                updParameters.Add("@FlashcardFront", front);
                updParameters.Add("@FlashcardBack", back);
                connection.Execute(@"
                    UPDATE Flashcard
                    SET FlashcardFront = @FlashcardFront, FlashcardBack = @FlashcardBack
                    WHERE FlashcardId = @FlashcardId",
                    updParameters);

                AnsiConsole.MarkupLine("[yellow]Flashcard updated successfully![/]");
                AnsiConsole.MarkupLine($"[green]{front}[/] => [green]{back}[/]");
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
