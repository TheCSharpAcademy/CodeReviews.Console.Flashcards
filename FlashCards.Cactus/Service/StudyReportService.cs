using System.Globalization;
using FlashCards.Cactus.Dao;
using Spectre.Console;

namespace FlashCards.Cactus.Service;

public class StudyReportService
{
    public StudyReportService(StudySessionDao studySessionDao)
    {
        StudySessionDao = studySessionDao;
    }

    public StudySessionDao StudySessionDao { get; set; }

    public void ShowStudyReportInSpecificYear()
    {
        Console.WriteLine("Input a year in format YYYY");
        string? yearStr = Console.ReadLine();
        DateTime parseTmp;
        do
        {
            bool res = DateTime.TryParseExact(yearStr, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseTmp);
            if (res) break;
            Console.WriteLine("The year your input is invalid. Please input a valid year in format YYYY.");
            yearStr = Console.ReadLine();
        } while (true);

        int year = parseTmp.Year;

        var reports = StudySessionDao.GetAverageScorePerMontInSpecYear(year);

        var table = new Table();
        table.Title($"Average score per month in {year}");
        table.AddColumns("Stack", "Jan.", "Feb.", "Mar.", "Apr.", "May.", "Jun.", "Jul.", "Aug.", "Sep.", "Oct.", "Nov.", "Dec.");
        foreach (var report in reports)
        {
            table.AddRow(report.Item1,
                report.Item2[0], report.Item2[1], report.Item2[2],
                report.Item2[3], report.Item2[4], report.Item2[5],
                report.Item2[6], report.Item2[7], report.Item2[8],
                report.Item2[9], report.Item2[10], report.Item2[11]);
        }
        AnsiConsole.Write(table);
    }
}

