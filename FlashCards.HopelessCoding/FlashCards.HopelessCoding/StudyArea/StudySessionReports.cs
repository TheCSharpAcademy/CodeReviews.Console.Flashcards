using DbHelpers.HopelessCoding;
using Spectre.Console;
using System.Data.SqlClient;
using System.Data;
using HelperMethods.HopelessCoding;

namespace FlashCards.HopelessCoding.Study;

internal class StudySessionReports
{
    internal static void StudySessionReport(bool isAverage = true)
    {
        Console.Clear();
        string year = Helpers.GetValidYearFromUser();
        string title = isAverage ? $"Average score per month in {year}" : $"Number of sessions in {year}";
        DataTable dataTable = RetrieveReportDataFromDatabase(year, isAverage);

        if (dataTable != null)
        {
            var reportTable = GenerateReportTable(title, dataTable);
            AnsiConsole.Write(reportTable);
            Console.Write("\nPress any key to continue.");
            Console.ReadLine();
        }
    }

    private static Table GenerateReportTable(string title, DataTable dataTable)
    {
        var reportTable = new Table();
        reportTable.Title = new TableTitle($"[yellow1]{title}[/]");
        reportTable.Border = TableBorder.Rounded;

        foreach (DataColumn column in dataTable.Columns)
        {
            reportTable.AddColumn($"[gold1]{column.ColumnName}[/]").Alignment(Justify.Center);
        }

        foreach (DataRow row in dataTable.Rows)
        {
            var rowValues = new List<string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.Ordinal == 0)
                {
                    rowValues.Add(row[column].ToString());
                }
                else
                {
                    rowValues.Add(row[column] != DBNull.Value ? Convert.ToDouble(row[column]).ToString("N1") : "0");
                }

            }
            reportTable.AddRow(rowValues.ToArray());
        }
        return reportTable;
    }

    private static DataTable RetrieveReportDataFromDatabase(string year, bool average = true)
    {
        string reportQuery;

        if (average == true)
        {
            reportQuery = "SELECT * " +
                            "FROM(" +
                                "SELECT Stack AS 'Stack Name', DATENAME(MONTH, Date) AS MonthName, CAST(Score AS float) AS Score " +
                                "FROM StudySessions " +
                                "WHERE YEAR(Date) = @Year " +
                            ") AS PivotData " +
                            "PIVOT(" +
                               "AVG(Score) " +
                               "FOR MonthName IN([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])) " +
                               "AS PivotTable;";
        }
        else
        {
            reportQuery = "SELECT * " +
                              "FROM(" +
                                "SELECT Stack AS 'Stack Name', DATENAME(MONTH, Date) AS MonthName, COUNT(*) AS TaskCount " +
                                "FROM StudySessions " +
                                "WHERE YEAR(Date) = @Year " +
                                "GROUP BY Stack, DATENAME(MONTH, Date)" +
                             ") AS PivotData " +
                             "PIVOT(" +
                                "SUM(TaskCount) " +
                                "FOR MonthName IN([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])) " +
                                "AS PivotTable;";
        }

        using (SqlConnection connection = new SqlConnection(DatabaseHelpers.connectionString))
        {

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(reportQuery, connection);
                command.Parameters.AddWithValue("@Year", year);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"[red]$An error occurred: {ex.Message}[/]\nPress any key to continue."));
                Console.ReadLine();
                return null;
            }
        }
    }
}
