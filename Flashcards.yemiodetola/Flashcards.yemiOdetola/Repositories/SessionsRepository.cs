using Flashcards.yemiOdetola.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.yemiOdetola.Repositories;

public class SessionsRepository
{
  string ConnectionString = "Server=localhost,1433;Database=FlashCards;User Id=sa;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;";
  public List<Session> GetSessions()
  {
    List<Session> sessions = new List<Session>();

    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"SELECT * FROM sessions JOIN stacks ON sessions.stack_id = stacks.id";

    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
    if (sqlDataReader.HasRows)
    {
      while (sqlDataReader.Read())
      {
        Session session = new Session(
          sqlDataReader.GetInt32(1),
          sqlDataReader.GetInt32(2),
          sqlDataReader.GetInt32(4),
          sqlDataReader.GetDateTime(3),
          sqlDataReader.GetInt32(5),
          sqlDataReader.GetString(6)
        );
        sessions.Add(session);
      }
    }

    return sessions;
  }

  public List<SessionDto> GetSessionsReport(int year)
  {
    List<SessionDto> stackSessions = new List<SessionDto>();

    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = @$"SELECT name, [1] AS January, [2] AS February, [3] AS March, 
        [4] AS April, [5] AS May, [6] AS June, [7] AS July, [8] AS August, [9] AS September,
        [10] AS October, [11] AS November, [12] AS December FROM (SELECT name, MONTH(datetime) AS month FROM sessions
        JOIN stacks ON sessions.stack_id = stacks.id WHERE YEAR(datetime) = {year}) AS tab PIVOT (COUNT(month) FOR month 
        IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) AS pvt";

    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
    if (sqlDataReader.HasRows)
    {
      while (sqlDataReader.Read())
      {
        Dictionary<string, int> sessions = new Dictionary<string, int>
        {
            { "January", sqlDataReader.GetInt32(1) },
            { "February", sqlDataReader.GetInt32(2) },
            { "March", sqlDataReader.GetInt32(3) },
            { "April", sqlDataReader.GetInt32(4) },
            { "May", sqlDataReader.GetInt32(5) },
            { "June", sqlDataReader.GetInt32(6) },
            { "July", sqlDataReader.GetInt32(7) },
            { "August", sqlDataReader.GetInt32(8) },
            { "September", sqlDataReader.GetInt32(9) },
            { "October", sqlDataReader.GetInt32(10) },
            { "November", sqlDataReader.GetInt32(11) },
            { "December", sqlDataReader.GetInt32(12) }
        };

        SessionDto stackSession = new SessionDto(sqlDataReader.GetString(0), sessions);

        stackSessions.Add(stackSession);
      }
    }

    return stackSessions;
  }

  public List<ScoreDto> GetSessionsScore(int year)
  {
    List<ScoreDto> stackScores = new List<ScoreDto>();

    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $@"SELECT name, ISNULL([1], 0) AS January, ISNULL([2], 0) AS February, ISNULL([3], 0) AS March,
        ISNULL([4], 0) AS April, ISNULL([5], 0) AS May, ISNULL([6], 0) AS June, ISNULL([7], 0) AS July, ISNULL([8], 0) AS August,
        ISNULL([9], 0) AS September, ISNULL([10], 0) AS October, ISNULL([11], 0) AS November, ISNULL([12], 0) AS December FROM
        (SELECT name, MONTH(datetime) AS month, ROUND(CAST(score as FLOAT) / max_score * 100, 2) AS percentage FROM sessions
        JOIN stacks ON sessions.stack_id = stacks.id WHERE YEAR(datetime) = {year}) AS tab
        PIVOT (AVG(percentage) FOR month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) AS pvt;";

    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
    if (sqlDataReader.HasRows)
    {
      while (sqlDataReader.Read())
      {
        Dictionary<string, double> scores = new Dictionary<string, double>
        {
            { "January", sqlDataReader.GetDouble(1) },
            { "February", sqlDataReader.GetDouble(2) },
            { "March", sqlDataReader.GetDouble(3) },
            { "April", sqlDataReader.GetDouble(4) },
            { "May", sqlDataReader.GetDouble(5) },
            { "June", sqlDataReader.GetDouble(6) },
            { "July", sqlDataReader.GetDouble(7) },
            { "August", sqlDataReader.GetDouble(8) },
            { "September", sqlDataReader.GetDouble(9) },
            { "October", sqlDataReader.GetDouble(10) },
            { "November", sqlDataReader.GetDouble(11) },
            { "December", sqlDataReader.GetDouble(12) }
        };

        ScoreDto stackScore = new ScoreDto(sqlDataReader.GetString(0), scores);

        stackScores.Add(stackScore);
      }
    }

    return stackScores;
  }

  public void AddSession(int stackId, int score, DateTime dateTime, int maxScore)
  {
    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    string date = dateTime.ToString("yyyy-MM-dd HH:MM:ss");
    sqlCommand.CommandText = $"INSERT INTO sessions (stack_id, score, datetime, max_score) VALUES ({stackId}, {score}, '{date}', {maxScore})";
    sqlCommand.ExecuteNonQuery();
  }

}
