using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Report.Strategies;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Report;

/// <summary>
/// Represents an average yearly report view.
/// </summary>
internal class AverageYearlyReportView : ReportViewBaseClass<IStackMonthlySessions>
{
    public AverageYearlyReportView(IReportStrategy<IStackMonthlySessions> reportStrategy) : base(reportStrategy)
    {
    }

    private protected override Table PopulateReportTable(Table table)
    {
        foreach (var session in ReportStrategy.Data)
        {
            table.AddRow(
                session.StackName!,
                session.January.ToString(),
                session.February.ToString(),
                session.March.ToString(),
                session.April.ToString(),
                session.May.ToString(),
                session.June.ToString(),
                session.July.ToString(),
                session.August.ToString(),
                session.September.ToString(),
                session.October.ToString(),
                session.November.ToString(),
                session.December.ToString()
            );
        }

        return table;
    }
}