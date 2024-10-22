using Flashcards.harris_andy.Classes;
using Spectre.Console;

namespace Flashcards.harris_andy;

public class DisplayData
{
    public void MainMenu()
    {
        Console.Clear();
        Console.WriteLine(
            "--------------------------------------------------\n" +
            "\n\t\tMAIN MENU\n\n" +
            "\tWhat would you like to do?\n\n" +
            "\tType 0 to Close Application\n" +
            "\tType 1 to Study Flashcards\n" +
            "\tType 2 to Create a New Flash Card\n" +
            "\tType 3 to Create a New Stack\n" +
            "\tType 4 to Delete a Stack\n" +
            "\tType 5 to View Study Sessions\n" +
            "\tType 6 to View Study Sessions COUNT by Month\n" +
            "\tType 7 to View Study Sessions GRADES by Month\n" +
            "\tType 8 to Add Fake Data\n" +
            "\tType 9 to Add Fake Study Sessions\n" +
            "--------------------------------------------------\n");
    }

    public void ShowStackNames(List<Stack> stackData)
    {
        var table = new Table();
        bool isAlternateRow = false;

        table.BorderColor(Color.DarkSlateGray1);
        table.Border(TableBorder.Rounded);
        table.AddColumn(new TableColumn("[cyan1]ID[/]").LeftAligned());
        table.AddColumn(new TableColumn("[green1]Name[/]").RightAligned());

        foreach (Stack stack in stackData)
        {
            var color = isAlternateRow ? "grey" : "blue";
            table.AddRow(
                $"[{color}]{stack.Id}[/]",
                $"[{color}]{stack.Name ?? "N/A"}[/]"
            );
            isAlternateRow = !isAlternateRow;
        }
        Console.Clear();
        AnsiConsole.Write(table);
    }

    public void ShowStackMessage(List<Stack> stackData, int stackID, string message)
    {
        var stackName = stackData
                .Where(s => s.Id == stackID)
                .Select(s => s.Name)
                .FirstOrDefault();
        Console.WriteLine($"{message} {stackName}.");
        Thread.Sleep(2000);
    }

    public void ShowStudySessions(List<StudySessionDTO> records, string name)
    {
        var table = new Table();
        bool isAlternateRow = false;
        table.Title("Study Sessions");
        table.BorderColor(Color.DarkSlateGray1);
        table.Border(TableBorder.Rounded);
        table.AddColumn(new TableColumn("[cyan1]Date[/]").LeftAligned());
        table.AddColumn(new TableColumn("[green1]Subject[/]").RightAligned());
        table.AddColumn(new TableColumn("[blue1]Score[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow1]Questions[/]").RightAligned());
        table.AddColumn(new TableColumn("[red1]% Correct[/]").LeftAligned());

        foreach (StudySessionDTO record in records)
        {
            string grade = (record.Score / (float)record.Questions).ToString("P1");
            var color = isAlternateRow ? "grey" : "blue";
            table.AddRow(
                $"[{color}]{record.Date.ToShortDateString()}[/]",
                $"[{color}]{name}[/]",
                $"[{color}]{record.Score}[/]",
                $"[{color}]{record.Questions}[/]",
                $"[{color}]{grade}[/]"
            );
            isAlternateRow = !isAlternateRow;
        }
        Console.Clear();
        AnsiConsole.Write(table);
    }

    public void DisplayCard(string text, int index)
    {
        Console.Clear();
        Panel panel = new Panel(text);
        panel.Header = new PanelHeader($"[blue]Flash Card: {index}[/]");
        panel.HeaderAlignment(Justify.Center);
        panel.Border = BoxBorder.Double;
        panel.Border = BoxBorder.Rounded;
        panel.Padding = new Padding(10, 5, 10, 5);
        AnsiConsole.Write(panel);
    }

    public void DisplayScore(int score, int questions)
    {
        Console.Clear();
        string text = $"You got {score}/{questions} correct.";
        Panel panel = new Panel(text);
        panel.Header = new PanelHeader($"[yellow]Score[/]");
        panel.HeaderAlignment(Justify.Center);
        panel.Border = BoxBorder.Heavy;
        panel.BorderColor(Color.Yellow);
        panel.Padding = new Padding(10, 5, 10, 5);
        AnsiConsole.Write(panel);
    }

    public void NothingFound(string item)
    {
        Console.Clear();
        Console.WriteLine($"No {item} found!");
        Thread.Sleep(2000);
    }

    public void DisplayStudyReport(List<StudyReport> records, string title, string caster)
    {
        var table = new Table();
        bool isAlternateRow = false;
        table.Title(title);
        table.BorderColor(Color.DarkSlateGray1);
        table.Border(TableBorder.Rounded);
        table.AddColumn(new TableColumn("[cyan1]Stack Name[/]").LeftAligned());
        table.AddColumn(new TableColumn("[green1]January[/]").RightAligned());
        table.AddColumn(new TableColumn("[blue1]February[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow1]March[/]").RightAligned());
        table.AddColumn(new TableColumn("[red1]April[/]").RightAligned());
        table.AddColumn(new TableColumn("[cyan1]May[/]").RightAligned());
        table.AddColumn(new TableColumn("[green1]June[/]").RightAligned());
        table.AddColumn(new TableColumn("[blue1]July[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow1]August[/]").RightAligned());
        table.AddColumn(new TableColumn("[green1]September[/]").RightAligned());
        table.AddColumn(new TableColumn("[blue1]October[/]").RightAligned());
        table.AddColumn(new TableColumn("[cyan1]November[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow]December[/]").LeftAligned());

        if (caster == "counts")
        {
            foreach (StudyReport record in records)
            {
                var color = isAlternateRow ? "grey" : "blue";
                table.AddRow(
                    $"[{color}]{record.StackName}[/]",
                    $"[{color}]{record.January}[/]",
                    $"[{color}]{record.February}[/]",
                    $"[{color}]{record.March}[/]",
                    $"[{color}]{record.April}[/]",
                    $"[{color}]{record.May}[/]",
                    $"[{color}]{record.June}[/]",
                    $"[{color}]{record.July}[/]",
                    $"[{color}]{record.August}[/]",
                    $"[{color}]{record.September}[/]",
                    $"[{color}]{record.October}[/]",
                    $"[{color}]{record.November}[/]",
                    $"[{color}]{record.December}[/]"
                );
                isAlternateRow = !isAlternateRow;
            }
        }
        if (caster == "grades")
        {
            foreach (StudyReport record in records)
            {
                var color = isAlternateRow ? "grey" : "blue";
                table.AddRow(
                    $"[{color}]{record.StackName}[/]",
                    FormatGrade(record.January, color),
                    FormatGrade(record.February, color),
                    FormatGrade(record.March, color),
                    FormatGrade(record.April, color),
                    FormatGrade(record.May, color),
                    FormatGrade(record.June, color),
                    FormatGrade(record.July, color),
                    FormatGrade(record.August, color),
                    FormatGrade(record.September, color),
                    FormatGrade(record.October, color),
                    FormatGrade(record.November, color),
                    FormatGrade(record.December, color)
                );
                isAlternateRow = !isAlternateRow;
            }
        }
        Console.Clear();
        AnsiConsole.Write(table);
    }

    private string FormatGrade(object value, string color)
    {
        return $"[{color}]{(value == null ? "0.0%" : Convert.ToDouble(value).ToString("P1"))}[/]";
    }
}
