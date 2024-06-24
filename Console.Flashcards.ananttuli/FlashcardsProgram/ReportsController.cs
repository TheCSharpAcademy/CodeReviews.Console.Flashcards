using System.Globalization;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Reports;
using Spectre.Console;

namespace FlashcardsProgram;

public class ReportsController(ReportsRepository reportsRepository)
{
    public ReportsRepository reportsRepo = reportsRepository;

    public void AverageScorePerMonthPerStack()
    {
        AnsiConsole.MarkupLine("Average score per month per stack");
        int year = AnsiConsole.Ask<int>("Enter year? ");

        var scoresByStackByMonth = reportsRepo.GetSessionScoresPerMonthPerStack(year);

        if (scoresByStackByMonth.Count == 0)
        {
            AnsiConsole.MarkupLine(Utils.Text.Markup("No session data", "red"));
            return;
        }

        var monthColumns = DateTimeFormatInfo.CurrentInfo.MonthNames.Where(name => name.Trim().Length > 0);

        var table = new Table();

        table.AddColumns(["Name", .. monthColumns]);

        foreach (var result in scoresByStackByMonth)
        {
            try
            {
                var stackName = result?["Name"]?.ToString() ?? "-";
                var monthAverages = monthColumns.Select(col =>
                    {
                        var average = result?[col] ?? null;
                        return average == null ? "-" : $"{average}%";
                    });

                table.AddRow([
                    stackName,
                    ..monthAverages
                ]);
            }
            catch
            {
                table.AddEmptyRow();
                continue;
            }
        }

        AnsiConsole.Write(table);
    }
}