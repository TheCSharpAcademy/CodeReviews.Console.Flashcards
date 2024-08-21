using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Report.Strategies;
using Flashcards.Eddyfadeev.Interfaces.View.Report;
using Flashcards.Eddyfadeev.Services;
using Flashcards.Eddyfadeev.View.Report;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.Report;

/// <summary>
/// Generates reports for study sessions.
/// </summary>
internal class ReportGenerator<TEntity> where TEntity : class
{
    private readonly IReportStrategy<TEntity> _reportStrategy;
    private readonly ReportType _reportType;

    public ReportGenerator(IReportStrategy<TEntity> reportStrategy, ReportType reportType)
    {
        _reportStrategy = reportStrategy;
        _reportType = reportType;
        SetLicence();
    }

    /// <summary>
    /// Gets the report to display based on the type of entity and report type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <returns>The report to display as a table.</returns>
    public Table GetReportToDisplay()
    {
        var reportViewMappings = new Dictionary<(Type, ReportType), Func<IReportView>>
        {
            { (typeof(IStudySession), ReportType.FullReport), () => new FullReportView((IReportStrategy<IStudySession>)_reportStrategy) },
            { (typeof(IStudySession), ReportType.ReportByStack), () => new ReportByStackView((IReportStrategy<IStudySession>)_reportStrategy) },
            { (typeof(IStackMonthlySessions), ReportType.AverageYearlyReport), () => new AverageYearlyReportView((IReportStrategy<IStackMonthlySessions>)_reportStrategy) }
        };
        
        if (!reportViewMappings.TryGetValue((typeof(TEntity), _reportType), out var reportView))
        {
            AnsiConsole.MarkupLine(Messages.Messages.UnsupportedReportTypeMessage);
            GeneralHelperService.ShowContinueMessage();
        }

        return reportView!().GetReportToDisplay();
    }

    ///<summary>
    /// Saves the report to a PDF file.
    /// </summary>
    /// <remarks>
    /// This method asks the user if they want to save the report.
    /// If the user confirms, it generates the report document and saves it to a PDF file on the desktop.
    /// The file name is based on the document title and the current date.
    /// </remarks>
    public void SaveReportToPdf()
    {
        if (!AskToSaveReport())
        {
            return;
        }

        var pdfDocument = GenerateReportToFile();
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, $"{_reportStrategy.DocumentTitle}-{DateTime.Today.ToShortDateString()}.pdf");

        pdfDocument.GeneratePdf(filePath);
        AnsiConsole.MarkupLine($"Saved report to [bold]{filePath}[/]");
    }
    
    private ReportDocument<TEntity> GenerateReportToFile()
    {
        var document = new ReportDocument<TEntity>(_reportStrategy);
        return document;
    }

    private static bool AskToSaveReport()
    {
        var confirm = AnsiConsole.Confirm(Messages.Messages.SaveAsPdfMessage);
        return confirm;
    }

    private static void SetLicence() =>
        QuestPDF.Settings.License = LicenseType.Community;
}