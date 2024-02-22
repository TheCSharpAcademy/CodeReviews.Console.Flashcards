using System.Data.SqlClient;
using Flashcards.Mateusz_Platek.Models;

namespace Flashcards.Mateusz_Platek.Repositories;

public class SessionsRepository
{
    public List<Session> GetSessions()
    {
        List<Session> sessions = new List<Session>();
        
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"SELECT * FROM sessions JOIN stacks ON sessions.stack_id = stacks.stack_id";
        
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        if (sqlDataReader.HasRows)
        {
            while (sqlDataReader.Read())
            {
                Session session = new Session(
                    sqlDataReader.GetInt32(0),
                    sqlDataReader.GetInt32(1),
                    sqlDataReader.GetInt32(2),
                    sqlDataReader.GetDateTime(3),
                    sqlDataReader.GetInt32(4),
                    sqlDataReader.GetString(6)
                    );
                sessions.Add(session);
            }
        }

        return sessions;
    }

    public List<StackSessionDTO> GetSessionsReport(int year)
    {
        List<StackSessionDTO> stackSessions = new List<StackSessionDTO>();

        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = @$"SELECT name, [1] AS January, [2] AS February, [3] AS March, 
        [4] AS April, [5] AS May, [6] AS June, [7] AS July, [8] AS August, [9] AS September,
        [10] AS October, [11] AS November, [12] AS December FROM (SELECT name, MONTH(datetime) AS month FROM sessions
        JOIN stacks ON sessions.stack_id = stacks.stack_id WHERE YEAR(datetime) = {year}) AS tab PIVOT (COUNT(month) FOR month 
        IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) AS pvt";
        
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        if (sqlDataReader.HasRows)
        {
            while (sqlDataReader.Read())
            {
                Dictionary<string, int> sessions = new Dictionary<string, int>();
                
                sessions.Add("January", sqlDataReader.GetInt32(1));
                sessions.Add("February", sqlDataReader.GetInt32(2));
                sessions.Add("March", sqlDataReader.GetInt32(3));
                sessions.Add("April", sqlDataReader.GetInt32(4));
                sessions.Add("May", sqlDataReader.GetInt32(5));
                sessions.Add("June", sqlDataReader.GetInt32(6));
                sessions.Add("July", sqlDataReader.GetInt32(7));
                sessions.Add("August", sqlDataReader.GetInt32(8));
                sessions.Add("September", sqlDataReader.GetInt32(9));
                sessions.Add("October", sqlDataReader.GetInt32(10));
                sessions.Add("November", sqlDataReader.GetInt32(11));
                sessions.Add("December", sqlDataReader.GetInt32(12));

                StackSessionDTO stackSession = new StackSessionDTO(sqlDataReader.GetString(0), sessions);
                
                stackSessions.Add(stackSession);
            }
        }
        
        return stackSessions;
    }

    public List<StackScoreDTO> GetSessionsScore(int year)
    {
        List<StackScoreDTO> stackScores = new List<StackScoreDTO>();
        
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $@"SELECT name, ISNULL([1], 0) AS January, ISNULL([2], 0) AS February, ISNULL([3], 0) AS March,
        ISNULL([4], 0) AS April, ISNULL([5], 0) AS May, ISNULL([6], 0) AS June, ISNULL([7], 0) AS July, ISNULL([8], 0) AS August,
        ISNULL([9], 0) AS September, ISNULL([10], 0) AS October, ISNULL([11], 0) AS November, ISNULL([12], 0) AS December FROM
        (SELECT name, MONTH(datetime) AS month, ROUND(CAST(score as FLOAT) / max_score * 100, 2) AS percentage FROM sessions
        JOIN stacks ON sessions.stack_id = stacks.stack_id WHERE YEAR(datetime) = {year}) AS tab
        PIVOT (AVG(percentage) FOR month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) AS pvt;";

        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        if (sqlDataReader.HasRows)
        {
            while (sqlDataReader.Read())
            {
                Dictionary<string, double> scores = new Dictionary<string, double>();
                
                scores.Add("January", sqlDataReader.GetDouble(1));
                scores.Add("February", sqlDataReader.GetDouble(2));
                scores.Add("March", sqlDataReader.GetDouble(3));
                scores.Add("April", sqlDataReader.GetDouble(4));
                scores.Add("May", sqlDataReader.GetDouble(5));
                scores.Add("June", sqlDataReader.GetDouble(6));
                scores.Add("July", sqlDataReader.GetDouble(7));
                scores.Add("August", sqlDataReader.GetDouble(8));
                scores.Add("September", sqlDataReader.GetDouble(9));
                scores.Add("October", sqlDataReader.GetDouble(10));
                scores.Add("November", sqlDataReader.GetDouble(11));
                scores.Add("December", sqlDataReader.GetDouble(12));

                StackScoreDTO stackScore = new StackScoreDTO(sqlDataReader.GetString(0), scores);

                stackScores.Add(stackScore);
            }
        }
        
        return stackScores;
    }
    
    public void AddSession(int stackId, int score, DateTime dateTime, int maxScore)
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        string date = dateTime.ToString("yyyy-MM-dd HH:MM:ss");
        sqlCommand.CommandText = $"INSERT INTO sessions (stack_id, score, datetime, max_score) VALUES ({stackId}, {score}, '{date}', {maxScore})";
        sqlCommand.ExecuteNonQuery();
    }
}