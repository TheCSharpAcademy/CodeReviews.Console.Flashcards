using Flashcards.Interfaces.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Flashcards.Report;

/// <summary>
/// Represents a report document for generating study history reports.
/// </summary>
internal class ReportDocument : IDocument
{
    private readonly List<IStudySession> _studySessions;
    
    public ReportDocument(List<IStudySession> studySessions)
    {
        _studySessions = studySessions;
    }

    /// <summary>
    /// Composes a study history report document.
    /// </summary>
    /// <param name="container">The document container to compose the report in.</param>
    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(20);
                page.Header().Text("Study History").AlignCenter();
                page.Content()
                    .Border(1)
                    .Table(table =>
                    {
                        DefineColumns(table);
                        DefineHeader(table);
                        PopulateTable(table);
                    });
            });
    }
    
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    private static void DefineColumns(TableDescriptor table)
    {
        table
            .ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(); // Date column
                columns.RelativeColumn(); // Stack column
                columns.RelativeColumn(); // Result column
                columns.RelativeColumn(); // Percentage column
                columns.RelativeColumn(); // Duration column
            });
    }
    
    private static void DefineHeader(TableDescriptor table)
    {
        table.Header(header =>
        {
            header.Cell().Padding(5).Text("Date").Bold();
            header.Cell().Padding(5).Text("Stack").Bold();
            header.Cell().Padding(5).Text("Result").Bold();
            header.Cell().Padding(5).Text("Percentage").Bold();
            header.Cell().Padding(5).Text("Duration").Bold();

            header
                .Cell()
                .ColumnSpan(5)
                .ExtendHorizontal()
                .Height(1)
                .Background(Colors.Black);
        });
    }

    private void PopulateTable(TableDescriptor table)
    {
        foreach (var studySession in _studySessions)
        {
            AddTableRow(
                table, 
                studySession.Date.ToShortDateString(), 
                studySession.StackName!, 
                $"{ studySession.CorrectAnswers } out of { studySession.Questions }", 
                $"{ studySession.Percentage }%", 
                studySession.Time.ToString("g")[..7]);
        }
    }

    private static IContainer CellStyle(IContainer container) =>
        container.Padding(5);
    
    private static void AddTableRow(TableDescriptor table, string date, string stack, string result, string percentage, string duration)
    {
        table.Cell().Element(CellStyle).Text(date);
        table.Cell().Element(CellStyle).Text(stack);
        table.Cell().Element(CellStyle).Text(result);
        table.Cell().Element(CellStyle).Text(percentage);
        table.Cell().Element(CellStyle).Text(duration);
    }
}