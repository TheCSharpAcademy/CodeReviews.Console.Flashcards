using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Flashcards.Eddyfadeev.Interfaces.Report.Strategies;

/// <summary>
/// Represents the interface for a report strategy.
/// </summary>
/// <typeparam name="TEntity">The type of entity the report strategy handles.</typeparam>
internal interface IReportStrategy<TEntity>
{
    internal List<TEntity> Data { get; }
    internal string[] ReportColumns { get; }
    internal string DocumentTitle { get; }
    internal PageSize PageSize { get; }

    /// <summary>
    /// Configures the table based on the report strategy.
    /// </summary>
    /// <param name="table">The table descriptor to configure.</param>
    internal void ConfigureTable(TableDescriptor table);

    /// <summary>
    /// Populates the table with data based on the report strategy.
    /// </summary>
    /// <param name="table">The table descriptor to populate.</param>
    internal void PopulateTable(TableDescriptor table);
}