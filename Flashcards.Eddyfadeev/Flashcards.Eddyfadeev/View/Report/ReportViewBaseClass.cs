using Flashcards.Eddyfadeev.Interfaces.Report.Strategies;
using Flashcards.Eddyfadeev.Interfaces.View.Report;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Report;

/// <summary>
/// Represents a base class for report views.
/// </summary>
/// <typeparam name="TEntity">The type of the entity that the report is based on.</typeparam>
internal abstract class ReportViewBaseClass<TEntity> : IReportView
{
    private protected IReportStrategy<TEntity> ReportStrategy { get; }
    
    protected ReportViewBaseClass(IReportStrategy<TEntity> reportStrategy)
    {
        ReportStrategy = reportStrategy;
    }

    /// <summary>
    /// Retrieves a table that represents the report to be displayed.
    /// </summary>
    /// <returns>
    /// The table object representing the report.
    /// </returns>
    public Table GetReportToDisplay()
    {
        var table = InitializeReportTable();
        table = PopulateReportTable(table);

        return table;
    }
    
    private Table InitializeReportTable()
    {
        var table = new Table
        {
            Border = TableBorder.Rounded,
            Title = new TableTitle(ReportStrategy.DocumentTitle)
        };
        table.AddColumns(ReportStrategy.ReportColumns);
        
        return table;
    }

    private protected abstract Table PopulateReportTable(Table table);
}