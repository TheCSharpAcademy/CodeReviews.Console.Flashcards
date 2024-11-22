using FlashCards.Controllers;
using FlashCards.Data;
using FlashCards.Utilities;
using FlashCards.Views;

namespace FlashCards.Services;

public static class ReportService
{
    private static readonly ReportController _reportController = new ReportController(new DatabaseService());

    internal static void SessionsReport(string stack)
    {
        string year = ReportExtensions.GetYear();
        int stack_id = FlashcardExtensions.GetStack_id(stack);
        var result = _reportController.GetSessionReportData(year, stack_id);
        var processedData = new List<dynamic>();
        if (result is IEnumerable<dynamic> enumerable)
        {
            foreach (var row in enumerable)
            {
                var processedRow = new Dictionary<string, object>();
                foreach (var property in (IDictionary<string, object>)row)
                {
                    processedRow[property.Key] = property.Value ?? 0;
                }
                processedData.Add(processedRow);
            }
        }
        TableVisualisation.ShowSessionReport(processedData, year, stack);
    }

    internal static void AverageScoreReport(string stack)
    {
        string year = ReportExtensions.GetYear();
        int stack_id = FlashcardExtensions.GetStack_id(stack);
        var result = _reportController.GetAverageReportData(year, stack_id);
        var processedData = new List<dynamic>();
        if (result is IEnumerable<dynamic> enumerable)
        {
            foreach (var row in enumerable)
            {
                var processedRow = new Dictionary<string, object>();
                foreach (var property in (IDictionary<string, object>)row)
                {
                    processedRow[property.Key] = property.Value ?? 0;
                }
                processedData.Add(processedRow);
            }
        }
        TableVisualisation.ShowAverageReport(result, year, stack);
    }
}