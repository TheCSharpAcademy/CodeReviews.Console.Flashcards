using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

internal class FlashcardRead
{
    internal static void ShowAllFlashcards()
    {
        Console.Clear();
        var stack = StackRead.GetStack();
        if (stack.StackName == null)
        {
            Console.Clear();
            return;
        }

        Console.Clear();
        var flashcards = GetListOfFlashcards(stack.StackId);
        if (DisplayInfoHelpers.NoRecordsAvailable(flashcards)) return;

        AnsiConsole.MarkupLine($"[yellow]{stack.StackName}[/] stack flashcards:\n");
        var table = new Table();
        int num = 1;
        table.AddColumn("Id.");
        table.AddColumn("Front");
        table.AddColumn("Back");
        foreach (var flashcard in flashcards)
        {
            table.AddRow(
                new Markup($"{num}"),
                new Markup($"[yellow]{flashcard.FlashcardFront}[/]"),
                new Markup($"{flashcard.FlashcardBack}"));
            num++;
        }
        AnsiConsole.Write(table);
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    internal static Dictionary<string, Flashcard> MakeFlashcardMap(int stackId)
    {
        var flashcards = GetListOfFlashcards(stackId);
        var flashcardList = MakeListOfFlashcards(flashcards);
        var flashcardMap = new Dictionary<string, Flashcard>();

        for (int i = 0; i < flashcards.Count; i++)
        {
            flashcardMap.Add(flashcardList[i], flashcards[i]);
        }
        return flashcardMap;
    }

    internal static List<string> MakeListOfFlashcards(List<Flashcard> flashcards)
    {
        var tableData = new List<string>();
        foreach (var flashcard in flashcards)
        {
            tableData.Add(
                $"[yellow]{flashcard.FlashcardFront}[/] => " +
                $"[yellow]{flashcard.FlashcardBack}[/] " +
                $"[{Console.BackgroundColor}] =>id:{flashcard.FlashcardId}[/]");
        }
        return tableData;
    }

    internal static List<Flashcard> GetListOfFlashcards(int stackId)
    {
        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", stackId);
            var query = @$"
                SELECT
                    FlashcardId AS FlashcardId,
                    FlashcardFront AS FlashcardFront,
                    FlashcardBack AS FlashcardBack
                FROM Flashcard
                WHERE StackId = @StackId
                ORDER BY FlashcardBack";
            return connection.Query<Flashcard>(query, parameters).ToList();
        }
        catch (SqlException ex)
        {
            DisplayErrorHelpers.SqlError(ex);
            return [];
        }
        catch (Exception ex)
        {
            DisplayErrorHelpers.GeneralError(ex);
            return [];
        }
    }
}
