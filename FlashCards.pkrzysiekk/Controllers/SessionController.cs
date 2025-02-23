using Dapper;
using FlashCards.Models;
using FlashCards.Models.Stack;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.Controllers
{
    public class SessionController : DbController
    {
        public void Insert(StackBO stack, SessionBO session)
        {
            session.StackId = stack.Id;
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "INSERT INTO sessions (Score,MaxScore,Date,StackId)" +
                    "VALUES (@Score,@MaxScore,@Date,@StackId)";
                try
                {
                    connection.Execute(sql, session);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine(ex.Message);
                }
            }
        }

        public IEnumerable<SessionBO>? GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM sessions";
                try
                {
                    return connection.Query<SessionBO>(sql).ToList();
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[Red]{ex.Message}[/]");
                    return null;
                }
            }
        }


        public IEnumerable<SessionStatistics> GetStatistics(StackBO session)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = @"
                 SELECT Name, 
                     COALESCE([1], 0) AS January,
                     COALESCE([2], 0) AS February,
                     COALESCE([3], 0) AS March,
                     COALESCE([4], 0) AS April,
                     COALESCE([5], 0) AS May,
                     COALESCE([6], 0) AS June,
                     COALESCE([7], 0) AS July,
                     COALESCE([8], 0) AS August,
                     COALESCE([9], 0) AS September,
                     COALESCE([10], 0) AS October,
                     COALESCE([11], 0) AS November,
                     COALESCE([12], 0) AS December
                 FROM (
                     SELECT 
                         stacks.Name,
                         MONTH(sessions.Date) AS Month,
                         Score
                     FROM sessions 
                     JOIN stacks ON sessions.StackId = stacks.Id
                     WHERE YEAR(GETDATE()) = 2025 AND StackId = @Id
                 ) AS SourceTable
                 PIVOT (
                     COUNT(Score) 
                     FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                 ) AS PivotTable
                 ORDER BY Name;";

                var result=connection.Query<SessionStatistics>(sql,session).ToList();
                return result;
            }

        }


    }
}
