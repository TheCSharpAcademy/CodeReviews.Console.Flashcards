using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards
{
    internal class Helpers
    {
        internal static bool DoesStacknameExist(string stackname)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                string sql = $"SELECT 1 FROM Stacks WHERE CONVERT(VARCHAR, Name) = '{stackname}'";
                using (SqlCommand command = new SqlCommand(sql, cnn))
                {
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        internal static List<string> GetQuestions(string stackToStudy)
        {
            List<string> questions = new List<string>();
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                string sql = $"SELECT Question FROM Flashcards " +
                    $"WHERE CONVERT(VARCHAR, Stackname) = '{stackToStudy}'";
                using (SqlCommand command = new SqlCommand(sql, cnn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return questions;
        }
        internal static List<string> GetAnswers(string stackToStudy)
        {
            List<string> answers = new List<string>();
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                string sql = $"SELECT Answer FROM Flashcards " +
                    $"WHERE CONVERT(VARCHAR, Stackname) = '{stackToStudy}'";
                using (SqlCommand command = new SqlCommand(sql, cnn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            answers.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return answers;
        }
        internal static void WriteScoreToDb(string scoreString, string stackname)
        {
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;

            cnn = new SqlConnection(connectionString);
            cnn.Open();
            sql = $"INSERT INTO Study (Date, Score, StackId) " +
                $"VALUES (GETDATE(), '{scoreString}', " +
                $"(SELECT Id FROM Stacks WHERE CONVERT(VARCHAR, Name) = '{stackname}'))";

            command = new SqlCommand(sql, cnn);
            int rowsEdited = command.ExecuteNonQuery();
        }
    }
}
