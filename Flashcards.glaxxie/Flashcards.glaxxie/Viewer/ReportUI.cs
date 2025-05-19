using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.Display;
using Flashcards.glaxxie.Prompts;
using Spectre.Console;

namespace Flashcards.glaxxie.Viewer;

internal class ReportUI
{
    internal static void Display(int year)
    {
        if (General.ConfirmationInput("Check average score?"))
            DisplayAverage(year);
        if (General.ConfirmationInput("Check sessions count score?"))
            DisplayCount(year);
    }

    internal static void DisplayAverage(int year)
    {
        var table = new Table
        {
            Title = new TableTitle($"[yellow]Average Score Per Month for: {year}[/]"),
            Border = TableBorder.Rounded
        };
        table.AddColumns("Stack", "January", "Feburary", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December");
        var obj = SessionController.GetAverageScore(year);
        foreach (var row in obj)
        {
            var dict = (IDictionary<string, object>)row;
            table.AddRow([.. dict.Values.Select(v => v?.ToString() ?? "n/a")]);
        }
        AnsiConsole.Write(table);
        Menu.ClearDisplay("Press any key to continue");
    }

    internal static void DisplayCount(int year)
    {
        var table = new Table
        {
            Title = new TableTitle($"[yellow]Number of Sessions for: {year}[/]"),
            Border = TableBorder.Rounded
        };
        table.AddColumns("Stack", "January", "Feburary", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December", "ALL");
        var obj = SessionController.GetSessionCountsByStack(year);
        var yearlyTotal = 0;
        foreach (var row in obj)
        {
            var dict = (IDictionary<string, object>)row;
            List<string> rowValues = [.. dict.Values.Select(v => v?.ToString() ?? "0")];
            var stackTotal = dict.Skip(1).Sum(item => (int)item.Value);
            rowValues.Add(stackTotal.ToString());
            table.AddRow(rowValues.ToArray());
            yearlyTotal += stackTotal;
        }
        table.AddEmptyRow();
        table.AddRow(["", "", "", "", "", "", "", "", "", "", "", "", "", yearlyTotal.ToString()]);
        AnsiConsole.Write(table);
        if (year == DateTime.Today.Year)
        {
            AnsiConsole.MarkupLine("[red]**Current year is still going on[/]");
        }
        Menu.ClearDisplay("Press any key to continue");
    }
}