using FlashCardsLibrary.Models;
using System.Data.SqlClient;

namespace FlashCardsLibrary
{
    public static class StudySessionController
    {
        private static string _connectionString = Database._connectionString;

        public static void AddSession(StudySession session)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string addCmd = @"INSERT INTO Sessions(Name,StackName,Date,Answers,Total) VALUES(@name,@stackname,@date,@answers,@total)";
                using (var cmd = new SqlCommand(addCmd, conn))
                {
                    cmd.Parameters.AddWithValue("name", session.Name);
                    cmd.Parameters.AddWithValue("stackname", session.Stack.Name);
                    cmd.Parameters.AddWithValue("date", session.Date);
                    cmd.Parameters.AddWithValue("answers", session.Answers);
                    cmd.Parameters.AddWithValue("total", session.Total);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<StudySession> GetSessions(Stack stack)
        {
            List<StudySession> sessions = new List<StudySession>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string getCmd = @"SELECT * FROM Sessions WHERE StackName = @stackname";
                using (var cmd = new SqlCommand(getCmd, conn))
                {
                    cmd.Parameters.AddWithValue("stackname",stack.Name);
                    var reader = cmd.ExecuteReader();
                   
                    while (reader.Read())
                    {
                        sessions.Add(new StudySession((string)reader["Name"],new Stack((string)reader["StackName"]),
                            (DateTime)reader["Date"], (int)reader["Answers"], (int)reader["Total"]));
                    }
                }
            }
            return sessions;
        }
    }
}
