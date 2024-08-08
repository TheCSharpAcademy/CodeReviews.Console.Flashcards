using FlashCards.kwm0304.Config;
using FlashCards.kwm0304.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.kwm0304.Repositories;

public class StudySessionRepository
{
  private readonly string _connString;
  public StudySessionRepository()
  {
    _connString = AppConfiguration.GetConnectionString("DefaultConnection");
  }

  public async Task CreateSessionAsync(int score, int stackId)
  {
    var sql = "INSERT INTO StudySessions (StackId, StudiedAt, Score) VALUES (@StackId, @StudiedAt, @Score)";

    using var connection = new SqlConnection(_connString);
    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@StackId", stackId);
    command.Parameters.AddWithValue("@StudiedAt", DateTime.Now);
    command.Parameters.AddWithValue("@Score", score);

    try
    {
      await connection.OpenAsync();
      await command.ExecuteNonQueryAsync();
      AnsiConsole.WriteLine("Study session created successfully");
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
      throw;
    }
  }
  public async Task<List<StudySession>> GetAllSessionsAsync()
  {
    List<StudySession> sessions = [];
    var sql = "SELECT * FROM StudySessions";
    using var connection = new SqlConnection(_connString);
    using var command = new SqlCommand(sql, connection);
    await connection.OpenAsync();
    var reader = command.ExecuteReader();
    while (reader.Read())
    {
      var session = new StudySession(
        Convert.ToInt32(reader["Score"]),
          Convert.ToInt32(reader["StackId"])
      )
      {
        StudySessionId = Convert.ToInt32(reader["StudySessionId"]),
        StudiedAt = Convert.ToDateTime(reader["StudiedAt"])
      };
      sessions.Add(session);
    }
    return sessions;
  }

  public async Task<List<StudySession>> GetAllSessionsByStackAsync(int stackId)
  {
    List<StudySession> sessions = [];
    var sql = "SELECT * FROM StudySessions WHERE StackId = @Id";
    using var connection = new SqlConnection(_connString);
    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@Id", stackId);
    try
    {
      await connection.OpenAsync();
      using var reader = await command.ExecuteReaderAsync();
      if (await reader.ReadAsync())
      {
        var session = new StudySession(
          Convert.ToInt32(reader["Score"]),
          Convert.ToInt32(reader["StackId"])
        )
        {
          StudySessionId = Convert.ToInt32(reader["StudySessionId"]),
          StudiedAt = Convert.ToDateTime(reader["StudiedAt"])
        };
        sessions.Add(session);
      }
      else
      {
        AnsiConsole.WriteLine("No sessions corresponding to that stack id");
      }
      return sessions;
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
      throw;
    }
  }

  internal async Task<List<ReportData>> GetReportByScore()
  {
    List<ReportData> reports = [];
    var sql = @"SELECT StackName,[1] AS Jan,[2] AS Feb,[3] AS Mar,
              [4] AS Apr,[5] AS May,[6] AS Jun,[7] AS Jul,[8] AS Aug,
              [9] AS Sep,[10] AS Oct,[11] AS Nov,[12] AS Dec
                FROM 
                  (SELECT 
                    s.StackName,
                    MONTH(ss.StudiedAt) AS Month,
                    AVG(ss.Score) AS AverageScore
                  FROM 
                    StudySessions ss
                  INNER JOIN 
                    Stacks s ON ss.StackId = s.StackId
                  GROUP BY 
                    s.StackName, MONTH(ss.StudiedAt)
                  ) AS SourceTable
                PIVOT
                (
                  AVG(AverageScore)
                  FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                ) AS PivotTable;";

    using var connection = new SqlConnection(_connString);
    using var command = new SqlCommand(sql, connection);
    try
    {
      await connection.OpenAsync();
      using var reader = await command.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        var report = new ReportData
        {
          StackName = reader["StackName"].ToString(),
          Jan = reader["Jan"] != DBNull.Value ? Convert.ToInt32(reader["Jan"]) : (int?)null,
          Feb = reader["Feb"] != DBNull.Value ? Convert.ToInt32(reader["Feb"]) : (int?)null,
          Mar = reader["Mar"] != DBNull.Value ? Convert.ToInt32(reader["Mar"]) : (int?)null,
          Apr = reader["Apr"] != DBNull.Value ? Convert.ToInt32(reader["Apr"]) : (int?)null,
          May = reader["May"] != DBNull.Value ? Convert.ToInt32(reader["May"]) : (int?)null,
          Jun = reader["Jun"] != DBNull.Value ? Convert.ToInt32(reader["Jun"]) : (int?)null,
          Jul = reader["Jul"] != DBNull.Value ? Convert.ToInt32(reader["Jul"]) : (int?)null,
          Aug = reader["Aug"] != DBNull.Value ? Convert.ToInt32(reader["Aug"]) : (int?)null,
          Sep = reader["Sep"] != DBNull.Value ? Convert.ToInt32(reader["Sep"]) : (int?)null,
          Oct = reader["Oct"] != DBNull.Value ? Convert.ToInt32(reader["Oct"]) : (int?)null,
          Nov = reader["Nov"] != DBNull.Value ? Convert.ToInt32(reader["Nov"]) : (int?)null,
          Dec = reader["Dec"] != DBNull.Value ? Convert.ToInt32(reader["Dec"]) : (int?)null
        };
        reports.Add(report);
      }
      return reports;
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
      throw;
    }
  }

  internal async Task<List<ReportData>> GetReportsByAttempt()
  {
    List<ReportData> reports = [];
    var sql = @"SELECT StackName,[1] AS Jan,[2] AS Feb,[3] AS Mar,
              [4] AS Apr,[5] AS May,[6] AS Jun,[7] AS Jul,[8] AS Aug,
              [9] AS Sep,[10] AS Oct,[11] AS Nov,[12] AS Dec
                FROM 
                  (SELECT 
                    s.StackName,
                    MONTH(ss.StudiedAt) AS Month,
                    COUNT(*) AS SessionCount
                  FROM 
                    StudySessions ss
                  INNER JOIN 
                    Stacks s ON ss.StackId = s.StackId
                  GROUP BY 
                    s.StackName, MONTH(ss.StudiedAt)
                  ) AS SourceTable
                PIVOT
                (
                  SUM(SessionCount) 
                  FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                ) AS PivotTable;";

    using var connection = new SqlConnection(_connString);
    using var command = new SqlCommand(sql, connection);
    try
    {
      await connection.OpenAsync();
      using var reader = await command.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        var report = new ReportData
        {
          StackName = reader["StackName"].ToString(),
          Jan = reader["Jan"] != DBNull.Value ? Convert.ToInt32(reader["Jan"]) : (int?)null,
          Feb = reader["Feb"] != DBNull.Value ? Convert.ToInt32(reader["Feb"]) : (int?)null,
          Mar = reader["Mar"] != DBNull.Value ? Convert.ToInt32(reader["Mar"]) : (int?)null,
          Apr = reader["Apr"] != DBNull.Value ? Convert.ToInt32(reader["Apr"]) : (int?)null,
          May = reader["May"] != DBNull.Value ? Convert.ToInt32(reader["May"]) : (int?)null,
          Jun = reader["Jun"] != DBNull.Value ? Convert.ToInt32(reader["Jun"]) : (int?)null,
          Jul = reader["Jul"] != DBNull.Value ? Convert.ToInt32(reader["Jul"]) : (int?)null,
          Aug = reader["Aug"] != DBNull.Value ? Convert.ToInt32(reader["Aug"]) : (int?)null,
          Sep = reader["Sep"] != DBNull.Value ? Convert.ToInt32(reader["Sep"]) : (int?)null,
          Oct = reader["Oct"] != DBNull.Value ? Convert.ToInt32(reader["Oct"]) : (int?)null,
          Nov = reader["Nov"] != DBNull.Value ? Convert.ToInt32(reader["Nov"]) : (int?)null,
          Dec = reader["Dec"] != DBNull.Value ? Convert.ToInt32(reader["Dec"]) : (int?)null
        };
        reports.Add(report);
      }
      return reports;
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
      throw;
    }
  }
}