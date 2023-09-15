namespace Flashcards;

using ConsoleTableExt;

class ReportView : BaseView
{
    private readonly ReportController controller;
    private readonly ReportType reportType;
    private readonly int? year;
    private readonly List<ReportRow> reportData;

    public ReportView(ReportController controller, ReportType reportType, int? year, List<ReportRow> reportData)
    {
        this.controller = controller;
        this.reportType = reportType;
        this.year = year;
        this.reportData = reportData;
    }

    public override void Body()
    {
        string reportTitle = reportType switch
        {
            ReportType.NumberOfSessionsPerMonthPerStack => "Number of sessions per month per stack",
            ReportType.AverageScorePerMonthPerStack => "Average score per month per stack",
            _ => "ERROR - Undefined Report"
        };
        Console. WriteLine($"Report '{reportTitle}' - Year {year}");
        if (reportData != null && reportData.Count > 0)
        {
            ConsoleTableBuilder.From(reportData).ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("No data found.");
        }

        Console.WriteLine("Press enter to return to menu.");
        Console.ReadLine();
        controller.ShowMenu();
    }
}