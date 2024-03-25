using Dapper;
using Flashcards.JaegerByte.DataModels;
using System.Data.SqlClient;

namespace Flashcards.JaegerByte.DatabaseHandler
{
    internal class DatabaseFlashcardHandler
    {
        public int MaxCharsQuestion { get; set; } = 30;
        public int MaxCharsAnswer { get; set; } = 500;
        public int MinChars { get; set; } = 3;
        public string? InvalidResponse { get; set; }

        public List<Flashcard> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                // zuerst alle flashcards ziehen und dann objekte erstellen
                connection.Open();
                List<Flashcard> flashcards = connection.Query<Flashcard>("SELECT * FROM tblFlashcards").ToList<Flashcard>();
                connection.Close();
                return flashcards;
            }
        }
        public void Insert(Flashcard flashcard, CardStack whereStack)
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();
                connection.Execute(@$"INSERT INTO tblFlashcards(Question, Answer, StackID)
                                    VALUES ('{flashcard.Question}','{flashcard.Answer}','{whereStack.StackID}')");
                connection.Close();
            }
        }
        public void Delete(Flashcard flashcard)
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();
                connection.Execute($"DELETE FROM tblFlashcards WHERE FlashcardID = '{flashcard.FlashcardID}'");
                connection.Close();
            }
        }
        public bool CheckInsertInput(string question, string answer)
        {
            if (question.Length > MaxCharsQuestion)
            {
                InvalidResponse = $"invalid input. Question too long (max {MaxCharsQuestion} chars).\nPress ANY key to return";
                return false;
            }
            if (answer.Length > MaxCharsAnswer)
            {
                InvalidResponse = $"invalid input. Answer too long (max {MaxCharsAnswer} chars.\nPress ANY key to return) ";
                return false;
            }
            if (question.Length < MinChars)
            {
                InvalidResponse = $"invalid input. Question too short (min {MinChars} chars).\nPress ANY key to return";
                return false;
            }
            if (answer.Length < MinChars)
            {
                InvalidResponse = $"invalid input. Answer too short (min {MinChars} chars.\nPress ANY key to return";
                return false;
            }
            return true;
        }
        public string GetInvalidResponse()
        {
            return InvalidResponse;
        }
    }
}
