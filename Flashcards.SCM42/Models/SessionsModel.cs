using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Flashcards;

public class SessionsModel
{
    static string connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;

    internal static List<Session> FetchSessions()
    {
        var sessionList = new List<Session>();
        var sqlCommand = $@"SELECT StackId,
                                   SessionId,
                                   StackName, 
                                   Date, 
                                   Points, 
                                   FlashcardsShown,
                            ROW_NUMBER() OVER (ORDER BY SessionId) AS SequentialId
                            FROM Sessions";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                var reader = connection.ExecuteReader(sqlCommand);

                while (reader.Read())
                {
                    sessionList.Add(
                        new Session
                        {
                            StackId = reader.GetInt32(0),
                            SessionId = reader.GetInt32(1),
                            StackName = reader.GetString(2),
                            Date = reader.GetDateTime(3).ToString("MM/dd/yyyy"),
                            Points = reader.GetInt32(4),
                            FlashcardsShown = reader.GetInt32(5),
                            RowNumber = reader.GetInt64(6),
                        });
                }
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }

            return sessionList;
        }
    }

    internal static void InsertSession(DateTime date, string? stackName, int stackId, int points, int flashcardsShown)
    {
        var parameters = new
        {
            Date = date,
            StackId = stackId,
            StackName = stackName,
            Points = points,
            FlashcardsShown = flashcardsShown
        };
        var sqlCommand = $@"INSERT INTO Sessions (Date, Points, StackName, StackId, FlashcardsShown)
                            VALUES (@Date, @Points, @StackName, @StackId, @FlashcardsShown)";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }
    }

    internal static List<ReportItem> FetchMonthlyAverage(string? year)
    {
        var parameter = new { Year = year };

        var reportList = new List<ReportItem>();
        var sqlCommand = $@"SELECT 
                            StackName,
                            ISNULL([JANUARY], 0) AS January,
                            ISNULL([FEBRUARY], 0) AS February,
                            ISNULL([MARCH], 0) AS March,
                            ISNULL([APRIL], 0) AS April,
                            ISNULL([MAY], 0) AS May,
                            ISNULL([JUNE], 0) AS June,
                            ISNULL([JULY], 0) AS July,
                            ISNULL([AUGUST], 0) AS August,
                            ISNULL([SEPTEMBER], 0) AS September,
                            ISNULL([OCTOBER], 0) AS October,
                            ISNULL([NOVEMBER], 0) AS November,
                            ISNULL([DECEMBER], 0) AS December	
                        FROM (SELECT 
                                StackName,
                                YEAR (Date) AS Year,
                                DATENAME(MONTH, Date) AS Month,
                                AVG(Points) AS TotalPoints
                            FROM Sessions
                            WHERE YEAR(Date) = @Year
                            GROUP BY
                                StackName,
                                YEAR(Date),
                                DATENAME(MONTH, Date)
                        ) AS MonthlyAverageData
                        PIVOT (
                                MAX(TotalPoints)
                                FOR Month IN ([January],[February],[March],[April],
                                    [May],[June],[July],[August],[September],[October],
                                    [November],[December]) 
                        ) AS PivotTable
                        ORDER BY 
                            StackName";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                var reader = connection.ExecuteReader(sqlCommand, parameter);

                while (reader.Read())
                {
                    reportList.Add(
                        new ReportItem
                        {
                            StackName = reader.GetString(0),
                            January = reader.GetInt32(1),
                            February = reader.GetInt32(2),
                            March = reader.GetInt32(3),
                            April = reader.GetInt32(4),
                            May = reader.GetInt32(5),
                            June = reader.GetInt32(6),
                            July = reader.GetInt32(7),
                            August = reader.GetInt32(8),
                            September = reader.GetInt32(9),
                            October = reader.GetInt32(10),
                            November = reader.GetInt32(11),
                            December = reader.GetInt32(12)
                        });
                }
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }

        return reportList;
    }

    internal static List<ReportItem> FetchMonthlySession(string? year)
    {
        var parameter = new { Year = year };

        var reportList = new List<ReportItem>();
        var sqlCommand = $@"SELECT 
                            StackName,
                            ISNULL([JANUARY], 0) AS January,
                            ISNULL([FEBRUARY], 0) AS February,
                            ISNULL([MARCH], 0) AS March,
                            ISNULL([APRIL], 0) AS April,
                            ISNULL([MAY], 0) AS May,
                            ISNULL([JUNE], 0) AS June,
                            ISNULL([JULY], 0) AS July,
                            ISNULL([AUGUST], 0) AS August,
                            ISNULL([SEPTEMBER], 0) AS September,
                            ISNULL([OCTOBER], 0) AS October,
                            ISNULL([NOVEMBER], 0) AS November,
                            ISNULL([DECEMBER], 0) AS December	
                        FROM (SELECT 
                                StackName,
                                YEAR (Date) AS Year,
                                DATENAME(MONTH, Date) AS Month,
                                COUNT(1) AS NUMBEROFSESSIONS
                            FROM Sessions
                            WHERE YEAR(Date) = @Year
                            GROUP BY
                                StackName,
                                YEAR(Date),
                                DATENAME(MONTH, Date)
                        ) AS MonthlyData
                        PIVOT (
                                SUM(NUMBEROFSESSIONS)
                                FOR Month IN ([January],[February],[March],[April],
                                    [May],[June],[July],[August],[September],[October],
                                    [November],[December]) 
                        ) AS PivotTable
                        ORDER BY 
                            StackName";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                var reader = connection.ExecuteReader(sqlCommand, parameter);

                while (reader.Read())
                {
                    reportList.Add(
                        new ReportItem
                        {
                            StackName = reader.GetString(0),
                            January = reader.GetInt32(1),
                            February = reader.GetInt32(2),
                            March = reader.GetInt32(3),
                            April = reader.GetInt32(4),
                            May = reader.GetInt32(5),
                            June = reader.GetInt32(6),
                            July = reader.GetInt32(7),
                            August = reader.GetInt32(8),
                            September = reader.GetInt32(9),
                            October = reader.GetInt32(10),
                            November = reader.GetInt32(11),
                            December = reader.GetInt32(12)
                        });
                }
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }

        return reportList;
    }
}

public class Session
{
    public int StackId { get; set; }
    public int SessionId { get; set; }
    public string? StackName { get; set; }
    public string? Date { get; set; }
    public int Points { get; set; }
    public long RowNumber { get; set; }
    public int FlashcardsShown { get; set; }
}

public class ReportItem
{
    public string? StackName { get; set; }
    public int January { get; set; }
    public int February { get; set; }
    public int March { get; set; }
    public int April { get; set; }
    public int May { get; set; }
    public int June { get; set; }
    public int July { get; set; }
    public int August { get; set; }
    public int September { get; set; }
    public int October { get; set; }
    public int November { get; set; }
    public int December { get; set; }
}