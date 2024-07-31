using System.Globalization;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Reports;
using Spectre.Console;

namespace FlashcardsProgram;

public class ReportsController(ReportsRepository reportsRepository)
{
    public ReportsRepository reportsRepo = reportsRepository;

    public static string[] MonthColumns = DateTimeFormatInfo.CurrentInfo.MonthNames
        .Where(name => name.Trim().Length > 0).ToArray();

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

        var table = new Table();

        table.AddColumns(["Name", .. MonthColumns]);

        foreach (var result in scoresByStackByMonth)
        {
            var (success, row) = GetRowFromResult(result);
            if (success)
            {
                table.AddRow(row);
            }
            else
            {
                table.AddEmptyRow();
            }
        }

        AnsiConsole.Write(table);
    }

    private Tuple<bool, string[]> GetRowFromResult(Dictionary<string, object?>? result)
    {
        try
        {
            string stackName = result?["Name"]?.ToString() ?? "-";
            IEnumerable<string> monthAverages = MonthColumns.Select(col =>
                {
                    var average = result?[col]?.ToString() ?? "";
                    bool parseSuccess = decimal.TryParse(
                        average, out decimal parsedValue
                    );

                    return parseSuccess ?
                        (parsedValue.ToString("0.0") + "%") :
                        "-";
                });

            return new Tuple<bool, string[]>(true, [
                stackName,
                ..monthAverages
            ]);
        }
        catch
        {
            return new Tuple<bool, string[]>(false, []);
        }
    }
}