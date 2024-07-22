using Dapper;
using Flashcards.Arashi256.Config;
using Flashcards.Arashi256.Models;
using System.Data.SqlClient;

namespace Flashcards.Arashi256.Classes
{
    internal class FlashcardsDatabase
    {
        private Database _database;
        private DatabaseConnection _connection;

        public FlashcardsDatabase()
        {
            _database = new Database();
            _database.TestConnection();
            _connection = _database.GetConnection();
        }

        public bool CheckDuplicateFlashcard(int stackid, string front, string back)
        {
            string query = "SELECT Id, Subject FROM dbo.flashcards WHERE LOWER(Front) = LOWER(@Front) OR LOWER(Back) = LOWER(@Back)";
            var param = new DynamicParameters();
            param.Add("@StackId", stackid);
            param.Add("@Front", front);
            param.Add("@Back", back);
            List<Flashcard> flashcards = GetFlashcardResults(query, param);
            return flashcards.Count > 0 ? false : true;
        }

        public List<Flashcard> GetFlashcardResults(string query, DynamicParameters? parameters = null)
        {
            List<Flashcard> results = new List<Flashcard>();
            using (var connection = new SqlConnection(_connection.DatabaseConnectionString))
            {
                try
                {
                    results = connection.Query<Flashcard>(query, parameters).AsList();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("SQL ERROR: " + sqlEx.Message);
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    Console.WriteLine("INVALID OPERATION: " + invalidOpEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GENERAL ERROR: " + ex.Message);
                }
            }
            return results;
        }

        public int AddNewFlashcard(Flashcard f)
        {
            var sql = "INSERT INTO dbo.flashcards (StackId, Front, Back) VALUES (@StackId, @Front, @Back)";
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", f.StackId);
            parameters.Add("@Front", f.Front);
            parameters.Add("@Back", f.Back);
            return _database.ExecuteQuery(sql, parameters);
        }

        public int UpdateFlashcard(Flashcard f)
        {
            var sql = "UPDATE dbo.flashcards SET Front = @Front, Back = @Back WHERE StackId = @StackId AND Id = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", f.StackId);
            parameters.Add("@Id", f.Id);
            parameters.Add("@Front", f.Front);
            parameters.Add("@Back", f.Back);
            return _database.ExecuteQuery(sql, parameters);
        }

        public int DeleteFlashcard(Flashcard f)
        {
            var sql = "DELETE FROM dbo.flashcards WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", f.Id);
            return _database.ExecuteQuery(sql, parameters);
        }
    }
}
