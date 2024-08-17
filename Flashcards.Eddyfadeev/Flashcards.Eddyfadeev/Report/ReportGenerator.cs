using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Report;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Spectre.Console;

namespace Flashcards.Report;

/// <summary>
/// Generates reports for study sessions.
/// </summary>
internal class ReportGenerator : IReportGenerator
{
    public ReportGenerator()
    {
        SetLicence();
    }

    /// <summary>
    /// Retrieves the report to display as a table.
    /// </summary>
    /// <param name="studySessions">The list of study sessions.</param>
    /// <returns>The report as a table.</returns>
    public Table GetReportToDisplay(List<IStudySession> studySessions) => 
        GenerateReportTable(studySessions);

    /// <summary>
    /// Generates a report document with study session information and saves it to a PDF file.
    /// </summary>
    /// <param name="studySessions">A list of study sessions containing the information to be included in the report.</param>
    /// <returns>A document object representing the generated report.</returns>
    public IDocument GenerateReportToFile(List<IStudySession> studySessions)
    {
        var document = new ReportDocument(studySessions);

        return document;
    }

    /// <summary>
    /// Saves the full report to a PDF file.
    /// </summary>
    /// <param name="pdfDocument">The PDF document to save.</param>
    public void SaveFullReportToPdf(IDocument pdfDocument)
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, $"Study Report-{DateTime.Today.Date.ToShortDateString()}.pdf");
        pdfDocument.GeneratePdf(filePath);
        AnsiConsole.MarkupLine($"Saved report to [bold]{ filePath }[/]");
    }

    private static Table GenerateReportTable(List<IStudySession> studySessions)
    {
        var table = InitializeTable();
        table = PopulateTable(table, studySessions);

        return table;
    }

    private static Table InitializeTable()
    {
        var table = new Table().Title("[bold]Study History[/]");
        table.Border = TableBorder.Rounded;
        
        table.AddColumns("Date", "Stack", "Result", "Percentage", "Duration");
        
        return table;
    }

    private static Table PopulateTable(Table table, List<IStudySession> studySessions)
    {
        foreach (var session in studySessions)
        {
            table.AddRow(
                session.Date.ToShortDateString(),
                session.StackName!,
                $"{ session.CorrectAnswers } out of { session.Questions }",
                $"{ session.Percentage }%",
                session.Time.ToString("g")[..7]
            );
        }
        
        return table;
    }
    
    private static void SetLicence() =>
        QuestPDF.Settings.License = LicenseType.Community;
}