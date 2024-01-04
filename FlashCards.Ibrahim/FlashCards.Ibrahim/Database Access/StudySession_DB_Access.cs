using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FlashCards.Ibrahim.Models;

namespace FlashCards.Ibrahim.Database_Access
{
    public class StudySession_DB_Access
    {
        static string _connectionString;
        public StudySession_DB_Access(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void InsertStudySession(int Stacks_Id, DateTime date,int score)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"
                        INSERT INTO StudySession_Table (Stacks_Id,Score,Date) 
                        VALUES (@Stacks_Id,@Date,@Score)";
                command.Parameters.AddWithValue("@Stacks_Id", Stacks_Id);
                command.Parameters.AddWithValue("@Score", score);
                command.Parameters.AddWithValue("@Date", date);
                command.ExecuteNonQuery();
            }
        }

        public static List<StudySession> GetAllSessions()
        {
            List<StudySession> studySessions = new List<StudySession>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"SELECT * FROM StudySession_Table";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudySession session = new StudySession();
                        session.Id = reader.GetInt32(0);
                        session.Stacks_Id = reader.GetInt32(1);
                        session.Date = reader.GetDateTime(2); 
                        session.Score = reader.GetInt32(3);
                        studySessions.Add(session);
                    }
                }
            }
            return studySessions;
        }
    }
}
