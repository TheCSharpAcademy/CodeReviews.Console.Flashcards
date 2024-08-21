using Flashcards.Eddyfadeev.Interfaces.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Flashcards.Eddyfadeev.Report.Strategies;

/// <summary>
/// Represents a strategy for generating an average yearly report.
/// </summary>
internal sealed class AverageYearlyReportStrategy : ReportStrategyBaseClass<IStackMonthlySessions>
{
    public override List<IStackMonthlySessions> Data { get; }

    public override string[] ReportColumns =>
        [
            "Stack", "Jan.", "Feb.", "Mar.", 
            "Apr.", "May", "June", "July", 
            "Aug.", "Sept.", "Oct.", "Nov.", "Dec."
        ];

    public override string DocumentTitle { get; }
    public override PageSize PageSize => PageSizes.A4.Landscape();

    public AverageYearlyReportStrategy(List<IStackMonthlySessions> monthlySessions, IYear year)
    {
        Data = monthlySessions;
        DocumentTitle = $"Average Yearly Report for {year.ChosenYear}";
    }

    ///<summary>
    /// Configures the table in the report document with the necessary column headers and formatting.
    /// </summary>
    /// <param name="table">The table descriptor in the report document.</param>
    public override void ConfigureTable(TableDescriptor table)
    {
        DefineReportColumnsHeader(table);
        DefineReportColumns(table);
    }

    /// <summary>
    /// Populates a table in a report with data from a list of monthly sessions.
    /// </summary>
    /// <param name="table">The table descriptor of the report.</param>
    /// <remarks>
    /// This method retrieves data from a list of monthly sessions and populates the table in a report.
    /// It iterates through each monthly session and adds a row to the table for each session,
    /// with the stack name and the session data for each month.
    /// The table descriptor should be configured before calling this method.
    /// </remarks>
    public override void PopulateTable(TableDescriptor table)
    {
        foreach (var monthlySession in Data)
        {
            AddTableRow(
                table,
                monthlySession.StackName!,
                monthlySession.January.ToString(),
                monthlySession.February.ToString(),
                monthlySession.March.ToString(),
                monthlySession.April.ToString(),
                monthlySession.May.ToString(),
                monthlySession.June.ToString(),
                monthlySession.July.ToString(),
                monthlySession.August.ToString(),
                monthlySession.September.ToString(),
                monthlySession.October.ToString(),
                monthlySession.November.ToString(),
                monthlySession.December.ToString()
                );
        }
    }

    private protected override void DefineReportColumns(TableDescriptor table)
    {
        table
            .ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(2); // Stack column
                // Subtract 1 column for the correct number of columns
                for (int i = 0; i < ReportColumns.Length - 1; i++)
                {
                    columns.RelativeColumn(); // Month columns
                }
            });
    }
}