using System.Globalization;
using Spectre.Console;

namespace FlashCards.Cactus.Service;

public class StudyReportService
{
    public void ShowStudyReportInSpecificYear()
    {
        Console.WriteLine("Input a year in format YYYY");
        string? yearStr = Console.ReadLine();
        DateTime parseTmp;
        DateTime.TryParseExact(yearStr, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseTmp);
        int year = parseTmp.Year;

        var table = new Table();
        table.Title($"Average score per month in {year}");
        table.AddColumns("StackName", "Jan.", "Feb.", "Mar.", "Apr.", "May.", "Jun.", "Jul.", "Aug.", "Sep.", "Oct.", "Nov.", "Dec.");
        table.AddRow("Word", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        AnsiConsole.Write(table);
    }
}

