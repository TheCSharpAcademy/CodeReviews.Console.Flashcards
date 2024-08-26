using Spectre.Console;

namespace Flashcards;

public class VisualizationEngine
{
    static TableBorder borderStyle = ViewStyles.borderStyle;
    static Color foregroundColor = ViewStyles.foregroundColor;
    static int bottomPad = 1;

    internal static void DisplayFlashcards(List<FlashcardDTO> cardList, string? name)
    {
        var table = new Table();
        table.Border = borderStyle;
        var bottomPadding = new Padder(table).PadBottom(bottomPad);

        table.AddColumn($"[{foregroundColor}]Card #[/]");
        table.AddColumn($"[{foregroundColor}]Card Front[/]");
        table.AddColumn($"[{foregroundColor}]Card Back[/]");

        foreach (FlashcardDTO card in cardList)
        {
            table.AddRow($"{card.RowNumber}", $"{card.CardFront}", $"{card.CardBack}");
        }

        AnsiConsole.Write(table);
    }

    internal static void DisplayStacks(List<StackDTO> stackList)
    {
        var table = new Table();
        table.Border = borderStyle;
        var bottomPadding = new Padder(table).PadBottom(bottomPad);

        table.AddColumn($"[{foregroundColor}]Stack #[/]");
        table.AddColumn($"[{foregroundColor}]Stack Name[/]");
        table.AddColumn($"[{foregroundColor}]Card Quantity[/]");

        foreach (var stack in stackList)
        {
            table.AddRow($"{stack.RowNumber}", $"{stack.StackName}", $"{stack.CardQuantity}");
        }

        AnsiConsole.Write(table);
    }

    internal static void DisplayAverageScoreTable(List<ReportItem> report, string year)
    {
        var table = new Table();
        table.Border = borderStyle;
        var bottomPadding = new Padder(table).PadBottom(bottomPad);
        table.Title($"Average Score Per Month For: {year}");

        table.AddColumn($"[{foregroundColor}]Stack Name[/]");
        table.AddColumn($"[{foregroundColor}]January[/]");
        table.AddColumn($"[{foregroundColor}]February[/]");
        table.AddColumn($"[{foregroundColor}]March[/]");
        table.AddColumn($"[{foregroundColor}]April[/]");
        table.AddColumn($"[{foregroundColor}]May[/]");
        table.AddColumn($"[{foregroundColor}]June[/]");
        table.AddColumn($"[{foregroundColor}]July[/]");
        table.AddColumn($"[{foregroundColor}]August[/]");
        table.AddColumn($"[{foregroundColor}]September[/]");
        table.AddColumn($"[{foregroundColor}]October[/]");
        table.AddColumn($"[{foregroundColor}]November[/]");
        table.AddColumn($"[{foregroundColor}]December[/]");

        foreach (var reportItem in report)
        {
            table.AddRow($"{reportItem.StackName}", $"{reportItem.January}", $"{reportItem.February}",
                         $"{reportItem.March}", $"{reportItem.April}", $"{reportItem.May}", $"{reportItem.June}",
                         $"{reportItem.July}", $"{reportItem.August}", $"{reportItem.September}",
                         $"{reportItem.October}", $"{reportItem.November}", $"{reportItem.December}");
        }

        AnsiConsole.Write(table);
    }

    internal static void DisplaySessionsTable(List<ReportItem> report, string? year)
    {
        var table = new Table();
        table.Border = borderStyle;
        var bottomPadding = new Padder(table).PadBottom(bottomPad);
        table.Title($"Sessions Per Month For: {year}");

        table.AddColumn($"[{foregroundColor}]Stack Name[/]");
        table.AddColumn($"[{foregroundColor}]January[/]");
        table.AddColumn($"[{foregroundColor}]February[/]");
        table.AddColumn($"[{foregroundColor}]March[/]");
        table.AddColumn($"[{foregroundColor}]April[/]");
        table.AddColumn($"[{foregroundColor}]May[/]");
        table.AddColumn($"[{foregroundColor}]June[/]");
        table.AddColumn($"[{foregroundColor}]July[/]");
        table.AddColumn($"[{foregroundColor}]August[/]");
        table.AddColumn($"[{foregroundColor}]September[/]");
        table.AddColumn($"[{foregroundColor}]October[/]");
        table.AddColumn($"[{foregroundColor}]November[/]");
        table.AddColumn($"[{foregroundColor}]December[/]");

        foreach (var reportItem in report)
        {
            table.AddRow($"{reportItem.StackName}", $"{reportItem.January}", $"{reportItem.February}",
                         $"{reportItem.March}", $"{reportItem.April}", $"{reportItem.May}", $"{reportItem.June}",
                         $"{reportItem.July}", $"{reportItem.August}", $"{reportItem.September}",
                         $"{reportItem.October}", $"{reportItem.November}", $"{reportItem.December}");
        }

        AnsiConsole.Write(table);
    }

    internal static void DisplaySessionsTable(List<Session> sessionsList)
    {
        var table = new Table();
        table.Border = borderStyle;
        var bottomPadding = new Padder(table).PadBottom(bottomPad);

        table.AddColumn($"[{foregroundColor}]Session #[/]");
        table.AddColumn($"[{foregroundColor}]Stack[/]");
        table.AddColumn($"[{foregroundColor}]Date[/]");
        table.AddColumn($"[{foregroundColor}]Correct[/]");
        table.AddColumn($"[{foregroundColor}]Flashcards Shown[/]");

        foreach (var session in sessionsList)
        {
            table.AddRow($"{session.RowNumber}", $"{session.StackName}", $"{session.Date}", $"{session.Points}", $"{session.FlashcardsShown}");
        }

        AnsiConsole.Write(table);
    }

    internal static void DisplayCardFront(Flashcard card)
    {
        var table = new Table();
        table.Border = borderStyle;
        var bottomPadding = new Padder(table).PadBottom(bottomPad);

        table.AddColumn($"[{foregroundColor}]Front[/]");
        table.AddRow($"{card.CardFront}");

        AnsiConsole.Write(table);
    }

    internal static void DisplayCardBack(Flashcard card)
    {
        var table = new Table();
        table.Border = borderStyle;
        var bottomPadding = new Padder(table).PadBottom(bottomPad);

        table.AddColumn($"[{foregroundColor}]Back[/]");
        table.AddRow($"{card.CardBack}");

        AnsiConsole.Write(table);
    }
}