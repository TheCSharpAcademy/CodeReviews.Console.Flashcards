using FlashcardsAssist.DreamFXX.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Data;
using System.Text;

namespace FlashcardsAssist.DreamFXX.Services;
public class ReportingService
{
    private readonly DatabaseService _dbService;
    private readonly string _connectionString;

    public ReportingService(DatabaseService dbService, IConfiguration configuration)
    {
        _dbService = dbService;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidExpressionException("Connection string not found.");
    }

    public async Task ViewSessionsPerMonthReportAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT 
                    s.Name AS StackName,
                    PIVOT (
                        COUNT(ss.Id)
                        FOR MONTH(ss.StudyDate) IN (
                            [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]
                        )
                    ) AS PivotTable
                FROM 
                    Stacks s
                LEFT JOIN 
                    StudySessions ss ON s.Id = ss.StackId AND YEAR(ss.StudyDate) = YEAR(GETDATE())
                GROUP BY 
                    s.Name
                ORDER BY 
                    s.Name
            ", connection);

            using var reader = await cmd.ExecuteReaderAsync();
            var dt = new DataTable();
            dt.Load(reader);

            var table = new Table()
                .Title("[yellow]Sessions per Month per Stack[/]")
                .AddColumn(new TableColumn("Stack Name").LeftAligned());

            // Add month columns
            string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            for (int i = 0; i < 12; i++)
            {
                table.AddColumn(new TableColumn(months[i]).Centered());
            }

            // Add data rows
            foreach (DataRow row in dt.Rows)
            {
                var rowData = new List<string> { row["StackName"].ToString() };
                
                for (int i = 1; i <= 12; i++)
                {
                    var value = row[i] == DBNull.Value ? "0" : row[i].ToString();
                    rowData.Add(value);
                }
                
                table.AddRow(rowData.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }

    public async Task ViewAverageScorePerMonthReportAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT 
                    s.Name AS StackName,
                    PIVOT (
                        AVG(ss.Score)
                        FOR MONTH(ss.StudyDate) IN (
                            [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]
                        )
                    ) AS PivotTable
                FROM 
                    Stacks s
                LEFT JOIN 
                    StudySessions ss ON s.Id = ss.StackId AND YEAR(ss.StudyDate) = YEAR(GETDATE())
                GROUP BY 
                    s.Name
                ORDER BY 
                    s.Name
            ", connection);

            using var reader = await cmd.ExecuteReaderAsync();
            var dt = new DataTable();
            dt.Load(reader);

            var table = new Table()
                .Title("[yellow]Average Score per Month per Stack[/]")
                .AddColumn(new TableColumn("Stack Name").LeftAligned());

            // Add month columns
            string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            for (int i = 0; i < 12; i++)
            {
                table.AddColumn(new TableColumn(months[i]).Centered());
            }

            // Add data rows
            foreach (DataRow row in dt.Rows)
            {
                var rowData = new List<string> { row["StackName"].ToString() };
                
                for (int i = 1; i <= 12; i++)
                {
                    var value = row[i] == DBNull.Value ? "-" : Math.Round(Convert.ToDouble(row[i]), 1).ToString();
                    rowData.Add(value);
                }
                
                table.AddRow(rowData.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
} 