
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards;

internal static class VisualizationEngine
{
    public static Table CreateTable(string title="")
    {
        var table = new Table();
        table.Title(title.ToUpper());
        table.Border = TableBorder.Square;
        table.ShowRowSeparators = true;
        return table;
    }
    
    internal static void DisplayAllStacksYearlySessionReport(IEnumerable<AllStackYearlyReport> reports, string title)
    {
        var table = CreateTable(title);
        table.AddColumns(["Stack Name", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]);
        foreach (var report in reports)
        {
            table.AddRow(report.StackName, report.January, report.February, report.March, report.April, report.May, report.June, report.July, report.August, report.September, report.October, report.November, report.December);
        }
        AnsiConsole.Write(table);
    }

    internal static void DisplayContinueMessage()
    {
        AnsiConsole.Markup($"Press [bold blue]Enter[/] to continue\n".ToUpper());
        Console.ReadLine();
    }

    internal static void DisplayStudySessions(IEnumerable<StudySession>? studies, string stackName)
    {
        var table = CreateTable($"Study Sessions for [green]{stackName}[/] Stack");
        table.AddColumns(["Id", "Date", "Score", "Total Questions"]);
        foreach (var study in studies)
        {
            table.AddRow(study.Id.ToString(), study.StudyDate.ToString("dd-MM-yyyy"), study.Score.ToString(), study.TotalQuestions.ToString());
        }
        AnsiConsole.Write(table);
    }

    internal static void DisplayYearlySessionReport(StackYearlyReport report, string title)
    {
        var table = CreateTable(title);
        table.AddColumns(["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]);
        table.AddRow(report.January, report.February, report.March, report.April, report.May, report.June, report.July, report.August, report.September, report.October, report.November, report.December);
        AnsiConsole.Write(table);
    }

    internal static void ShowResultMessage(int result, string message)
    {
        if (result == 0)
        {
            AnsiConsole.Markup($"{message} [bold maroon]unsuccessfully[/]. Please read the errors.\n".ToUpper());
        }
        else
        {
            AnsiConsole.Markup($"{message} [bold green]successfully[/].\n".ToUpper());
        }
        VisualizationEngine.DisplayContinueMessage();
    }

}