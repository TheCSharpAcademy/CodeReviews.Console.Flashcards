using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Repositories;

/// <summary>
/// Provides data access methods for managing <see cref="StudySession"/> entities.
/// </summary>
public class StudySessionRepository : BaseRepository<StudySession>, IStudySessionRepository {
    /// <summary>
    /// Initializes a new instance of the <see cref="StudySessionRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to interact with the database.</param>
    public StudySessionRepository(AppDbContext dbContext) : base(dbContext) {
    }

    /// <summary>
    /// Retrieves monthly average scores for each stack for a specified year.
    /// </summary>
    /// <param name="year">The year for which to retrieve the monthly averages.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="ReportDto"/> objects representing the monthly averages.</returns>
    public async Task<List<ReportDto>> GetMonthlyAveragesAsync(int year) {
        var result = new List<ReportDto>();

        // Define the SQL query
        const string sql = """
                           
                                   DECLARE @Year INT = @YearParam;
                           
                                   SELECT 
                                       StackName,
                                       ROUND(ISNULL([1], 0), 2) AS Jan,
                                       ROUND(ISNULL([2], 0), 2) AS Feb,
                                       ROUND(ISNULL([3], 0), 2) AS Mar,
                                       ROUND(ISNULL([4], 0), 2) AS Apr,
                                       ROUND(ISNULL([5], 0), 2) AS May,
                                       ROUND(ISNULL([6], 0), 2) AS Jun,
                                       ROUND(ISNULL([7], 0), 2) AS Jul,
                                       ROUND(ISNULL([8], 0), 2) AS Aug,
                                       ROUND(ISNULL([9], 0), 2) AS Sep,
                                       ROUND(ISNULL([10], 0), 2) AS Oct,
                                       ROUND(ISNULL([11], 0), 2) AS Nov,
                                       ROUND(ISNULL([12], 0), 2) AS Dec
                                   FROM
                                   (
                                       SELECT 
                                           ss.StackId, 
                                           st.Name AS StackName,
                                           MONTH(ss.DateTime) AS Month,
                                           AVG((CAST(ss.Score AS FLOAT) / ss.TotalQuestions) * 100) AS AvgPer
                                       FROM 
                                           StudySessions ss
                                       JOIN 
                                           Stacks st ON ss.StackId = st.Id
                                       WHERE 
                                           YEAR(ss.DateTime) = @YearParam
                                       GROUP BY 
                                           ss.StackId, st.Name, MONTH(ss.DateTime)
                                   ) AS SourceTable
                                   PIVOT
                                   (
                                       AVG(AvgPer)
                                       FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                                   ) AS PivotTable;
                               
                           """;

        // Get the database connection
        var connection = DbContext.Database.GetDbConnection();

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = System.Data.CommandType.Text;
        command.Parameters.Add(new SqlParameter("@YearParam", year));

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            result.Add(new ReportDto {
                StackName = reader.GetString(reader.GetOrdinal("StackName")),
                Jan = reader.GetDouble(reader.GetOrdinal("Jan")),
                Feb = reader.GetDouble(reader.GetOrdinal("Feb")),
                Mar = reader.GetDouble(reader.GetOrdinal("Mar")),
                Apr = reader.GetDouble(reader.GetOrdinal("Apr")),
                May = reader.GetDouble(reader.GetOrdinal("May")),
                Jun = reader.GetDouble(reader.GetOrdinal("Jun")),
                Jul = reader.GetDouble(reader.GetOrdinal("Jul")),
                Aug = reader.GetDouble(reader.GetOrdinal("Aug")),
                Sep = reader.GetDouble(reader.GetOrdinal("Sep")),
                Oct = reader.GetDouble(reader.GetOrdinal("Oct")),
                Nov = reader.GetDouble(reader.GetOrdinal("Nov")),
                Dec = reader.GetDouble(reader.GetOrdinal("Dec"))
            });
        }
        await connection.CloseAsync();
        return result;
    }

    /// <summary>
    /// Retrieves the total number of study sessions for each stack for a specified year.
    /// </summary>
    /// <param name="year">The year for which to retrieve the total number of sessions.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="ReportDto"/> objects representing the total number of sessions.</returns>
    public async Task<List<ReportDto>> GetSumOfSessionsAsync(int year) {
        var result = new List<ReportDto>();

        // Define the SQL query
        const string sql = """
                           
                               DECLARE @Year INT = @YearParam;
                           
                               SELECT 
                                   StackName,
                                   ISNULL([1], 0) AS Jan,
                                   ISNULL([2], 0) AS Feb,
                                   ISNULL([3], 0) AS Mar,
                                   ISNULL([4], 0) AS Apr,
                                   ISNULL([5], 0) AS May,
                                   ISNULL([6], 0) AS Jun,
                                   ISNULL([7], 0) AS Jul,
                                   ISNULL([8], 0) AS Aug,
                                   ISNULL([9], 0) AS Sep,
                                   ISNULL([10], 0) AS Oct,
                                   ISNULL([11], 0) AS Nov,
                                   ISNULL([12], 0) AS Dec
                               FROM
                               (
                                   SELECT 
                                       ss.StackId, 
                                       st.Name AS StackName,
                                       MONTH(ss.DateTime) AS Month,
                                       COUNT(*) AS SessionsCount
                                   FROM 
                                       StudySessions ss
                                   JOIN 
                                       Stacks st ON ss.StackId = st.Id
                                   WHERE 
                                       YEAR(ss.DateTime) = @YearParam
                                   GROUP BY 
                                       ss.StackId, st.Name, MONTH(ss.DateTime)
                               ) AS SourceTable
                               PIVOT
                               (
                                   SUM(SessionsCount)
                                   FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                               ) AS PivotTable;

                           """;

        // Get the database connection
        var connection = DbContext.Database.GetDbConnection();

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = System.Data.CommandType.Text;
        command.Parameters.Add(new SqlParameter("@YearParam", year));

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            result.Add(new ReportDto {
                StackName = reader.GetString(reader.GetOrdinal("StackName")),
                Jan = reader.GetInt32(reader.GetOrdinal("Jan")),
                Feb = reader.GetInt32(reader.GetOrdinal("Feb")),
                Mar = reader.GetInt32(reader.GetOrdinal("Mar")),
                Apr = reader.GetInt32(reader.GetOrdinal("Apr")),
                May = reader.GetInt32(reader.GetOrdinal("May")),
                Jun = reader.GetInt32(reader.GetOrdinal("Jun")),
                Jul = reader.GetInt32(reader.GetOrdinal("Jul")),
                Aug = reader.GetInt32(reader.GetOrdinal("Aug")),
                Sep = reader.GetInt32(reader.GetOrdinal("Sep")),
                Oct = reader.GetInt32(reader.GetOrdinal("Oct")),
                Nov = reader.GetInt32(reader.GetOrdinal("Nov")),
                Dec = reader.GetInt32(reader.GetOrdinal("Dec"))
            });
        }
        await connection.CloseAsync();
        return result;
    }
}
