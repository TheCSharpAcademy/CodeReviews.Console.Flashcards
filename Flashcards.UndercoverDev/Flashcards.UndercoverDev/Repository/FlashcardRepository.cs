
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository
{
    public class FlashcardRepository : IFlashcardRepository
    {
        const string _databaseName = "FlashcardDB";
        private readonly string _connectionStringTemplate = ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
        private readonly string _connectionString;

        public FlashcardRepository()
        {
            _connectionString = string.Format(_connectionStringTemplate, _databaseName);
        }

        public void Post(FlashcardDTO newFlashcard)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            string createFlashcardQuery = $"INSERT INTO Flashcards(StackId, Question, Answer) VALUES('{newFlashcard.StackId}', '{newFlashcard.Question}', '{newFlashcard.Answer}');";
            connection.Execute(createFlashcardQuery);
        }

        public void Delete(Flashcard flashcard)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            string deleteFlashcardQuery = $"DELETE FROM Flashcards WHERE Id = '{flashcard.Id}';";
            connection.Execute(deleteFlashcardQuery);
        }

        public List<Flashcard> GetFlashcards()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string getFlashcardsQuery = $"SELECT * FROM Flashcards;";

            return connection.Query<Flashcard>(getFlashcardsQuery).ToList();
        }

        public List<Flashcard> GetFlashcardsByStackId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string getFlashcardsQuery = $"SELECT * FROM Flashcards WHERE StackId = '{id}';";

            return connection.Query<Flashcard>(getFlashcardsQuery).ToList();
        }
    }
}