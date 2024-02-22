using FlashCards.Cactus.DataModel;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace FlashCards.Cactus.Dao;

public class StudySessionDao
{
    public StudySessionDao(string DBConnStr)
    {
        DBConnectionStr = DBConnStr;
    }

    public string DBConnectionStr { get; set; }

    public int Insert(StudySession study)
    {
        int rowsAffected = -1;
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO StudySession(stackName, date, timeSpan, score) 
                                    VALUES('{study.StackName}', '{study.Date}', 
                                           '{study.Time}', '{study.Score}');";
            rowsAffected = command.ExecuteNonQuery();
        }
        return rowsAffected;
    }

    public List<StudySession> FindAll()
    {
        List<StudySession> studySessions = new List<StudySession>();
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT ss.ssid, ss.stackName, ss.date, ss.timeSpan, ss.score 
                                    FROM StudySession ss";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string stackName = reader.GetString(1);
                    DateTime date = reader.GetDateTime(2);
                    double time = reader.GetDouble(3);
                    int score = reader.GetInt32(4);

                    StudySession study = new StudySession(id, stackName, date, time, score);
                    studySessions.Add(study);
                }
            }
        }
        return studySessions;
    }

    public int DeleteById(int id)
    {
        int rowsAffected = -1;
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            string deleteSql = "DELETE FROM StudySession WHERE ssid = @id";
            SqlCommand deleteCommand = new SqlCommand(deleteSql, connection);
            deleteCommand.Parameters.AddWithValue("@id", id);
            rowsAffected = deleteCommand.ExecuteNonQuery();
        }
        return rowsAffected;
    }

    public List<Tuple<string, List<string>>> GetAverageScorePerMontInSpecYear(int year)
    {
        List<Tuple<string, List<string>>> results = new List<Tuple<string, List<string>>>();
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"SELECT * from 
                (SELECT stackName, MONTH(date) month, score FROM StudySession
                    WHERE YEAR(date) = {year}
                ) as nt
                PIVOT ( SUM(score) FOR month in 
                ([1], [2], [3], [4], [5], [6], [7], [8], 
                [9], [10], [11], [12]) ) as res; ";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string stackName = reader.GetString(0);
                    List<string> scores = new List<string>();
                    for (int i = 1; i <= 12; i++)
                    {
                        try
                        {
                            scores.Add(reader.GetInt32(i).ToString());
                        }
                        catch (SqlNullValueException)
                        {
                            scores.Add("0");
                        }
                    }
                    results.Add(new Tuple<string, List<string>>(stackName, scores));
                }
            }
        }
        return results;
    }
}

