using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Report.Strategies;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Report;

/// <summary>
/// Represents a report by stack view.
/// </summary>
internal class ReportByStackView : ReportViewBaseClass<IStudySession>
{
    public ReportByStackView(IReportStrategy<IStudySession> reportStrategy) : base(reportStrategy)
    {
    }

    private protected override Table PopulateReportTable(Table table)
    {
        foreach (var session in ReportStrategy.Data)
        {
            table.AddRow(
                session.Date.ToShortDateString(),
                $"{session.CorrectAnswers} out of {session.Questions}",
                $"{session.Percentage}%"
            );
        }

        return table;
    }
}