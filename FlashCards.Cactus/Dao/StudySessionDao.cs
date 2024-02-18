using FlashCards.Cactus.DataModel;
using Microsoft.Data.SqlClient;

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
            command.CommandText = $@"INSERT INTO StudySession(sid, date, timeSpan, score) 
                                    VALUES('{study.SId}', '{study.Date}', 
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
            command.CommandText = @"SELECT ss.ssid, ss.sid, ss.date, ss.timeSpan, ss.score, s.name  
                                    FROM StudySession ss, Stack s WHERE ss.sid = s.sid";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int sid = reader.GetInt32(1);
                    DateTime date = reader.GetDateTime(2);
                    double time = reader.GetDouble(3);
                    int score = reader.GetInt32(4);
                    string stackName = reader.GetString(5);
                    StudySession study = new StudySession(id, sid, stackName, date, time, score);
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
}

