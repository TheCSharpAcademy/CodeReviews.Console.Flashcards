using Spectre.Console;

namespace Flashcards.Eddyfadeev.Interfaces.View.Report;

/// <summary>
/// Represents a view for displaying a report.
/// </summary>
internal interface IReportView
{
    /// <summary>
    /// Retrieves a report to be displayed.
    /// </summary>
    /// <returns>The report table to be displayed.</returns>
    Table GetReportToDisplay();
}