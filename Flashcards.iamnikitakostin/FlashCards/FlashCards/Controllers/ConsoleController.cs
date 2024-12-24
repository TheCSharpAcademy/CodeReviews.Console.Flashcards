using Spectre.Console;

namespace FlashCards.Controllers;
public class ConsoleController
{
    protected static void DisplayMessage(string message, string color = "yellow")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
        AnsiConsole.WriteLine("Press any key to continue");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    protected static bool ConfirmDeletion(string itemName)
    {

        AnsiConsole.Clear();
        var confirm = AnsiConsole.Confirm($"Are you sure you want to delete [red]{itemName}[/]?");
        return confirm;
    }

    protected static void SuccessMessage(string message)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[green]{message}[/]");
        AnsiConsole.WriteLine("Press any key to continue");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    protected static void ErrorMessage(string message)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[red]{message}[/]");
        AnsiConsole.WriteLine("Press any key to continue");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    protected static Table DrawStackTable()
    {
        AnsiConsole.Clear();

        Table table = new();
        table.AddColumn(new TableColumn("Id").Centered());
        table.AddColumn(new TableColumn("Name").Centered());
        table.AddColumn(new TableColumn("# of Study Sessions").Centered());
        table.AddColumn(new TableColumn("# of FlashCards").Centered());

        return table;
    }

    protected static Table DrawFlashcardTable()
    {
        AnsiConsole.Clear();

        Table table = new();
        table.AddColumn(new TableColumn("Id").Centered());
        table.AddColumn(new TableColumn("Front").Centered());
        table.AddColumn(new TableColumn("Back").Centered());
        table.AddColumn(new TableColumn("Parent Stack").Centered());
        table.AddColumn(new TableColumn("Creation Date").Centered());

        return table;
    }

    protected static Table DrawFlashcardReviewTable()
    {
        AnsiConsole.Clear();

        Table table = new();
        table.AddColumn(new TableColumn("Front").Centered());

        return table;
    }

    protected static Table DrawStudyingSessionsTable()
    {
        Table table = new();

        table.AddColumn("Id");
        table.AddColumn("Stack Name");
        table.AddColumn("Start Date");
        table.AddColumn("End Date");
        table.AddColumn("Total Time");
        table.AddColumn("Cards completed");

        return table;
    }
}
