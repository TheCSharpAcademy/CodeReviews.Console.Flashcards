using Flashcards.Eddyfadeev.Interfaces.Models;
using QuestPDF.Infrastructure;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.Interfaces.Report;

/// <summary>
/// Represents a report generator used for generating study session reports.
/// </summary>
internal interface IReportGenerator
{
    /// <summary>
    /// Retrieves the report to display as a table.
    /// </summary>
    /// <param name="studySessions">The list of study sessions.</param>
    /// <returns>The report as a table.</returns>
    internal Table GetReportToDisplay(List<IStudySession> studySessions);

    /// <summary>
    /// Generates a report document with study session information and saves it to a PDF file.
    /// </summary>
    /// <param name="studySessions">A list of study sessions containing the information to be included in the report.</param>
    /// <returns>A document object representing the generated report.</returns>
    internal IDocument GenerateReportToFile(List<IStudySession> studySessions);

    /// <summary>
    /// Saves the full report to a PDF file.
    /// </summary>
    /// <param name="pdfDocument">The PDF document to save.</param>
    internal void SaveFullReportToPdf(IDocument pdfDocument);
}