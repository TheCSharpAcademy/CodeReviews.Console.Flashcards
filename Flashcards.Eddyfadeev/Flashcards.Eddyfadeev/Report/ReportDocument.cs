using Flashcards.Eddyfadeev.Interfaces.Report.Strategies;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Flashcards.Eddyfadeev.Report;

/// <summary>
/// Represents a report document for generating study history reports.
/// </summary>
internal class ReportDocument<TEntity> : IDocument
{
    private readonly IReportStrategy<TEntity> _reportStrategy;

    public ReportDocument(IReportStrategy<TEntity> reportStrategy)
    {
        _reportStrategy = reportStrategy;
    }

    /// <summary>
    /// Composes the report document based on the provided report strategy.
    /// </summary>
    /// <param name="container">The IDocumentContainer used to compose the report.</param>
    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(20);
                page.Header().Text(_reportStrategy.DocumentTitle).AlignCenter();
                page.Size(_reportStrategy.PageSize);
                page.Content()
                    .Border(1)
                    .Table(table =>
                    {
                        _reportStrategy.ConfigureTable(table);
                        _reportStrategy.PopulateTable(table);
                    });
            });
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
}