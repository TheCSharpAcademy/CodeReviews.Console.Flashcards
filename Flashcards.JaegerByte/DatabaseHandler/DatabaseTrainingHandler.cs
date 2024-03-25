using Dapper;
using Flashcards.JaegerByte.DataModels;
using System.Data.SqlClient;

namespace Flashcards.JaegerByte.DatabaseHandler
{
    internal class DatabaseTrainingHandler
    {
        public List<TrainingSession> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();
                List<TrainingSession> trainingSessions = connection.Query<TrainingSession>("SELECT * FROM tblTrainingSessions").ToList<TrainingSession>();
                connection.Close();
                return trainingSessions;
            }
        }

        public void InsertSession(TrainingSession session)
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();
                connection.Execute(@$"INSERT INTO tblTrainingSessions(StackID, StartDate, EndDate, CorrectAnswers, WrongAnswers)
                VALUES('{session.StackID}','{session.StartDate}','{session.EndDate}','{session.CorrectAnswers}','{session.WrongAnswers}')");
                connection.Close();
            }
        }
    }
}
