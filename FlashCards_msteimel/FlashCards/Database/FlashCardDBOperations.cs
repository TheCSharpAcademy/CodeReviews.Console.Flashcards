using FlashCards.Models;
using Microsoft.Data.SqlClient;

namespace FlashCards.Database
{
    internal static class FlashCardDBOperations
    {
        private static readonly string dataBaseName = "FlashCardsDB";
        private static readonly string dbConnectionString = $"Server=LAPTOP-LFCOM607;Database={dataBaseName};Integrated Security=true;TrustServerCertificate=True;";
        private static readonly string flashCardTableName = "FlashCards";
        internal static List<FlashCard> GetFlashCardsByStackId(int stackId)
        {
            var flashCards = new List<FlashCard>();

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT * FROM {flashCardTableName} WHERE StackId = @StackId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@StackId", stackId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var flashCard = new FlashCard
                            {
                                Id = reader.GetInt32(0),
                                Question = reader.GetString(1),
                                Answer = reader.GetString(2),
                                StackId = reader.GetInt32(3)
                            };
                            flashCards.Add(flashCard);
                        }
                    }
                }
            }

            return flashCards;
        }

        internal static void AddFlashCard(FlashCard flashCard)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string insertQuery = $"INSERT INTO {flashCardTableName} (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Question", flashCard.Question);
                    command.Parameters.AddWithValue("@Answer", flashCard.Answer);
                    command.Parameters.AddWithValue("@StackId", flashCard.StackId);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static void DeleteFlashCardByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string deleteQuery = $"DELETE FROM {flashCardTableName} WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static void EditFlashCard(FlashCard flashCard)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string updateQuery = @$"UPDATE {flashCardTableName} SET Question = @Question, 
                                     Answer = @Answer WHERE Id = @id AND StackID = @stackID ";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", flashCard.Id);
                    command.Parameters.AddWithValue("@stackId", flashCard.StackId);
                    command.Parameters.AddWithValue("@Question", flashCard.Question);
                    command.Parameters.AddWithValue("@Answer", flashCard.Answer);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static List<FlashCardDto> GetFlashCardDtoByStackId(int stackId)
        {
            var flashCards = new List<FlashCardDto>();

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT * FROM {flashCardTableName} WHERE StackId = @StackId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@StackId", stackId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var flashCard = new FlashCardDto
                            {
                                Question = reader.GetString(1),
                                Answer = reader.GetString(2),
                            };
                            flashCards.Add(flashCard);
                        }
                    }
                }
            }

            return flashCards;
        }
    }
}
