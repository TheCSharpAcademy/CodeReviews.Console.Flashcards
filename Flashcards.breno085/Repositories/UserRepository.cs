using Dapper;
using Microsoft.Data.SqlClient;
using flashcards.Models;
using flashcards.Utils;
using System.Configuration;

namespace flashcards.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        public void InsertFlashcardsDataForTests(List<Flashcards> flashcardsList)
        {
            var sql = "INSERT INTO Flashcards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)";

            using (var connection = new SqlConnection(connectionString))
            {
                foreach (var flashcard in flashcardsList)
                {
                    connection.Execute(sql, new { flashcard.Front, flashcard.Back, flashcard.StackId });
                }
            }
        }

        public void UpdateFlashcardsData(int id, string frontOrBack, string text)
        {
            string column = frontOrBack.ToLower() == "front" ? "Front" : "Back";
            var sql = $"UPDATE Flashcards SET {column} = @Text WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, new { Text = text, Id = id });
            }
        }

        public void DeleteFlashcardsData(int id)
        {
            var sql = "DELETE FROM Flashcards WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, new { Id = id });
            }
        }

        public void ViewFlashcardsFrontData(string stackName)
        {
            int stackId = GetStackId(stackName);

            var sql = $"SELECT Id, Front FROM Flashcards WHERE StackId = {stackId}";

            using (var connection = new SqlConnection(connectionString))
            {
                var flashcards = connection.Query<Flashcards>(sql);

                SpectreTable.FlashcardsFrontTable(flashcards);
            }
        }
        public List<int> GetFlashcardsId(string stackName)
        {
            int stackId = GetStackId(stackName);
            List<int> flashcardsId = new List<int>();

            var sql = $"SELECT Id FROM Flashcards WHERE StackId = {stackId}";

            using (var connection = new SqlConnection(connectionString))
            {
                var flashcards = connection.Query<int>(sql);

                foreach (int flashcard in flashcards)
                {
                    flashcardsId.Add(flashcard);
                }
            }
            return flashcardsId;
        }

        public void ViewAllFlashcardsData(string stackName)
        {
            int stackId = GetStackId(stackName);

            var sql = $"SELECT Id, Front, Back FROM Flashcards WHERE StackId = {stackId}";

            using (var connection = new SqlConnection(connectionString))
            {
                var flashcards = connection.Query<Flashcards>(sql);

                SpectreTable.FlashcardsTable(flashcards);
            }
        }

        public void InsertStudyData(string stackName, int score)
        {
            int stackId = GetStackId(stackName);

            var sql = $"INSERT INTO Study (Date, Score, StackId) VALUES (@Date, @Score, @StackId)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, new { Date = DateTime.Now, Score = score, StackId = stackId });
            }
        }

        public void ViewStudySessionData()
        {
            var sql = "SELECT * FROM Study";

            using (var connection = new SqlConnection(connectionString))
            {
                var study = connection.Query<Study>(sql);

                SpectreTable.StudyTable(study);
            }
        }

        public string GetFlashcardsBack(int id)
        {
            var backSql = "SELECT Back FROM Flashcards WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QuerySingle<string>(backSql, new { Id = id });
            }
        }

        public string GetFlashcardsFront(int id)
        {
            var frontSql = "SELECT Front FROM Flashcards WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QuerySingle<string>(frontSql, new { Id = id });
            }
        }

        public int GetStackId(string stackName)
        {
            int stackId;
            var stackIdSql = "SELECT Id FROM Stacks WHERE LanguageName = @StackName";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                stackId = connection.ExecuteScalar<int>(stackIdSql, new { StackName = stackName });
            }
            return stackId;
        }
        public void InsertFlashcardsData(string stackName, string front, string back)
        {
            int stackId = GetStackId(stackName);

            var sql = "INSERT INTO Flashcards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, new { Front = front, Back = back, StackId = stackId });
            }
        }

        public void InsertStackData(string stack)
        {
            var sql = "INSERT INTO Stacks (LanguageName) VALUES (@Stack)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, new { Stack = stack });
            }
        }

        public void DeleteStackData(string stack)
        {
            var sql = "DELETE FROM Stacks WHERE LanguageName = @Stack";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, new { Stack = stack });
            }

            int stackId = GetStackId(stack);

            var sql2 = "DELETE FROM Flashcards WHERE StackId = @StackId";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql2, new { StackId = stackId });
            }
        }

        public List<string> ViewStacksData()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM Stacks";
                var stacks = connection.Query<Stacks>(sql);

                List<string> stackNames = new List<string>();

                foreach (var stack in stacks)
                {
                    stackNames.Add(stack.LanguageName);
                }

                SpectreTable.StackTable(stacks);
                // Console.WriteLine("Id\tName");
                // foreach (var stack in stacks)
                // {
                //     stackNames.Add(stack.LanguageName);
                //     Console.WriteLine($"{stack.Id}\t{stack.LanguageName}");
                // }
                return stackNames;
            }
        }

    }
}