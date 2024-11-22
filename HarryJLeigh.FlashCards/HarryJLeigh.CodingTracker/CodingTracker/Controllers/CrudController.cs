using System.Data.SQLite;
using CodingTracker.Data;
using CodingTracker.Models;
using Dapper;

namespace CodingTracker.Controllers; 

public class CrudController
{
    private DataBaseService DbService { get; }

    public CrudController(DataBaseService dbService) => DbService = dbService;

    public List<CodingSession> GetSession(int id) =>
        QuerySessions($"SELECT * FROM CodingTracker WHERE Id = @id", new { id });

    public List<CodingSession> GetLastSession() =>
        QuerySessions($"SELECT * FROM CodingTracker ORDER BY Id DESC LIMIT 1");

    public List<CodingSession> GetAllSessions() => QuerySessions("SELECT * FROM CodingTracker");

    public List<CodingSession> GetSessionsByFilter(DateTime startDate, string orderByDirection)
    {
        string query = $@"
        SELECT * 
        FROM CodingTracker 
        WHERE StartTime >= @StartDate
        ORDER BY StartTime {orderByDirection}";
        return QuerySessions(query, new { StartDate = startDate });
    }

    public int GetTotalSessionReport(FilterOptions? filterChoice, DateTime? startDate = null)
    {
        int totalSessions = 0;
        if (filterChoice == null)
        {
            List<CodingSession> sessions = GetAllSessions();
            totalSessions = sessions.Count;
        }
        else
        {
            string query = @"
                        SELECT *, 
                        (SELECT COUNT(*) FROM CodingTracker WHERE StartTime >= @StartDate) 
                        FROM CodingTracker 
                        WHERE StartTime >= @StartDate";
            List<CodingSession> sessions = QuerySessions(query, new { StartDate = startDate });
            totalSessions = sessions.Count;
        }
        return totalSessions;
    }

    public double GetAverageHoursReport(FilterOptions? filterChoice, DateTime? startDate = null)
    {
        string query;
        double result;
        if (filterChoice == null)
        {
            query = @"SELECT AVG(Duration) FROM CodingTracker ";
            using var connection = DbService.GetConnection();
            result = connection.ExecuteScalar<double>(query);
        }
        else
        {
            query = @"SELECT AVG(Duration) FROM CodingTracker WHERE StartTime >= @StartDate";
            using var connection = DbService.GetConnection();
            result = connection.ExecuteScalar<double>(query, new { StartDate = startDate });
        }
        return result;
    }

    private List<CodingSession> QuerySessions(string query, object? parameters = null)
    {
        using var connection = DbService.GetConnection();
        return connection.Query<CodingSession>(query, parameters).ToList();
    }
    
    public void Insert(string startTime, string endTime, string duration)
    {
        using SQLiteConnection connection = DbService.GetConnection();
        var query = $"""
            INSERT INTO CodingTracker(StartTime, EndTime, Duration)
            VALUES
                (@StartTime, @EndTime, @Duration)
            """;
            var parameters = new { StartTime = startTime, EndTime = endTime, Duration = duration };
            connection.Execute(query, parameters);
        }
    
        public void UpdateStartTime(int id, string startTime)
        {
            using SQLiteConnection connection = DbService.GetConnection();
            var parameters = new { Id = id, StartTime = startTime };
            var query = $"""
                         UPDATE CodingTracker
                         SET StartTime = @StartTime
                         WHERE Id = @Id
                         """;
            connection.Execute(query, parameters);
        }

        public void UpdateEndTime(int id, string endTime)
        {
            using SQLiteConnection connection = DbService.GetConnection();
            var parameters = new { Id = id, EndTime = endTime };
            var query = $"""
                         UPDATE CodingTracker
                         SET EndTime = @EndTime
                         WHERE Id = @Id
                         """;
            connection.Execute(query, parameters);
        }

        public void UpdateDuration(int id, string duration)
        {
            using SQLiteConnection connection = DbService.GetConnection();
            var parameters = new { Id = id, Duration = duration };
            var query = $"""
                         UPDATE CodingTracker
                         SET Duration = @Duration
                         WHERE Id = @Id
                         """;
            connection.Execute(query, parameters);
        }
        
        public void Delete(int id)
        {
            using SQLiteConnection connection = DbService.GetConnection();
            var query = $"DELETE From CodingTracker where Id = @Id";
            var parameters = new { Id = id, };
            connection.Execute(query, parameters);
        }
        
        public double? GetGoalHoursComplete(DateTime startDate)
        {
            using SQLiteConnection connection = DbService.GetConnection();
            string query = @"
                             SELECT COALESCE(SUM(CAST(Duration AS REAL)), -1) FROM CodingTracker WHERE StartTime >= @StartDate
                            ";
            var parameters = new { StartDate = startDate };
            double goalHoursLeft = connection.QuerySingleOrDefault<double>(query, parameters);
            
            goalHoursLeft = Math.Round(goalHoursLeft, 2);
            return goalHoursLeft;
        }
    }