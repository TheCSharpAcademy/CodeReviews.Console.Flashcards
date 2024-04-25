using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Flashcards
{
    class FlashcardsController
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        internal List<FlashcardModel> ReadFlashcards(int id)
        {
            TableVisualisation tableVisualisation = new();
            List<FlashcardModel> flashcards = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM Flashcards WHERE StackId = {id}";

                flashcards = connection.Query<FlashcardModel>(query).ToList();

                connection.Close();
            }

            if (flashcards.Count > 0) tableVisualisation.ShowFlashcards(flashcards);

            return flashcards;
        }

        internal void InsertFlashcard(FlashcardModel flashcard)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"INSERT INTO Flashcards (StackId, Front, Back) 
                                        VALUES('{flashcard.StackId}', '{flashcard.Front}', '{flashcard.Back}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal int RemoveFlashcard(int stackId, int flashcardId)
        {
            int rowCount;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"DELETE FROM Flashcards 
                                        WHERE StackId = {stackId} AND FlashcardId = {flashcardId}";

                rowCount = tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            return rowCount;
        }

        internal int UpdateFlashcard(int flashcardId, FlashcardModel flashcard)
        {
            int rowsAffected;
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"UPDATE Flashcards 
                                        SET Front = '{flashcard.Front}', 
                                        Back = '{flashcard.Back}' WHERE StackId = '{flashcard.StackId}' AND FlashcardId = '{flashcardId}'";

                rowsAffected = tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            return rowsAffected;
        }
    }
}