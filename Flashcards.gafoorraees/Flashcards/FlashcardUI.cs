using Flashcards.Tables;
using Spectre.Console;

namespace Flashcards;

internal class FlashcardUI
{
    public static void DisplayFlashcards(int stackId)
    {
        Console.Clear();
        var flashcards = FlashcardsTable.GetFlashcardsFromStack(stackId);
        var stackName = Stacks.ReturnStackName(stackId);

        if (flashcards.Count == 0)
        {
            Console.WriteLine("No available flashcards in this stack. Please add flashcards first.\n");

        }

        AnsiConsole.Write(new Markup($"[bold underline]Flashcards for stack: {stackName}[/]\n"));

        var table = new Table();

        table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Question[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Answer[/]").Centered());

        table.Border(TableBorder.Ascii);
        table.ShowHeaders = true;
        table.BorderColor(Color.Grey);
        table.ShowFooters = false;

        foreach (var flashcard in flashcards)
        {
            table.AddRow(
                flashcard.DisplayID.ToString(),
                flashcard.Question,
                flashcard.Answer
);
        }

        AnsiConsole.Write(table);
    }

    public static void ManageFlashcards()
    {
        Console.Clear();

        StacksUI.IndividualStacks();
    }
}
