using Flashcards.Eddyfadeev.Interfaces.Report.Strategies;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Flashcards.Eddyfadeev.Report.Strategies;

/// <summary>
/// The abstract base class for all report strategies in the Flashcards application.
/// </summary>
/// <typeparam name="TEntity">The type of entity that the report strategy handles.</typeparam>
internal abstract class ReportStrategyBaseClass<TEntity> : IReportStrategy<TEntity>
{
    public abstract List<TEntity> Data { get; }

    public abstract string[] ReportColumns { get; }
    public abstract string DocumentTitle { get; }
    public abstract PageSize PageSize { get; }

    ///<summary>
    /// Configures the table in the report document with the necessary column headers and formatting.
    /// </summary>
    /// <param name="table">The table descriptor in the report document.</param>
    public abstract void ConfigureTable(TableDescriptor table);

    /// <summary>
    /// Populates a table with data from a list of study sessions.
    /// </summary>
    /// <param name="table">The table descriptor to populate.</param>
    /// <remarks>
    /// This method retrieves data from a list of study sessions and populates the provided table.
    /// It iterates through each study session and adds a row to the table for each session,
    /// including the session date, score, and percentage.
    /// The table should be properly configured before calling this method.
    /// </remarks>
    public abstract void PopulateTable(TableDescriptor table);

    private protected void DefineReportColumnsHeader(TableDescriptor table)
    {
        table.Header(header =>
        {
            foreach (var columnName in ReportColumns)
            {
                header.Cell().Padding(5).Text(columnName).Bold();
            }
            
            header
                .Cell()
                .ColumnSpan((uint)ReportColumns.Length)
                .ExtendHorizontal()
                .Height(1)
                .Background(Colors.Black);
        });
    }

    private protected virtual void DefineReportColumns(TableDescriptor table)
    {
        table.ColumnsDefinition(col =>
        {
            foreach (var _ in ReportColumns)
            {
                col.RelativeColumn();
            }
        });
    }

    private protected static void AddTableRow(TableDescriptor table, params string[] rowNames)
    {
        foreach (var rowName in rowNames)
        {
            table.Cell().Element(CellStyle).Text(rowName);
        }
    }
    
    private protected static IContainer CellStyle(IContainer container) => container.Padding(5);
}
