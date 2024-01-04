using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Ibrahim.Models;
using static System.Net.Mime.MediaTypeNames;

namespace FlashCards.Ibrahim.Database_Acess
{
    internal class Flashcard_DB_Access
    {
        static string _connectionString;
        public Flashcard_DB_Access(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void Insert_Flashcard(int Stack_Id, string Front, string Back)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"
                        INSERT INTO Flashcards_Table (Stack_Id, Front, Back) 
                        VALUES (@Stack_Id, @Front, @Back)";
                command.Parameters.AddWithValue("@Stack_Id", Stack_Id);
                command.Parameters.AddWithValue("@Front", Front);
                command.Parameters.AddWithValue("@Back", Back);
                command.ExecuteNonQuery();
            }
        }
        public static void Update_Flashcard(int Id, string? front, string? back)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();

                if (front != null && back != null)
                {
                    command.CommandText = @"
                        UPDATE Flashcards_Table Set FRONT = @Front, BACK = @Back WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Front", front);
                    command.Parameters.AddWithValue("@Back", back);
                    command.ExecuteNonQuery();
                }
                else if (front != null && back == null)
                {
                    command.CommandText = @"
                        UPDATE Flashcards_Table Set FRONT = @Front WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Front", front);
                    command.ExecuteNonQuery();
                }
                else if(front == null && back != null)
                {
                    command.CommandText = @"
                        UPDATE Flashcards_Table Set BACK = @Back WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Back", back);
                    command.ExecuteNonQuery();
                } 
            }
        }
        public static void Delete_Flashcard(int Id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                {
                    command.CommandText = @"
                        DELETE FROM Flashcards_Table  WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static List<FlashcardDTO> GetAllFlashcards(int? Id)
        {
            List<FlashcardDTO> flashcards = new List<FlashcardDTO>();
            switch (Id)
            {
                case null:
                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command = sqlConnection.CreateCommand();
                            command.CommandText = @"SELECT * FROM Flashcards_Table";
                        using (var reader = command.ExecuteReader())
                        {
                            int displayOrder = 1;
                            while (reader.Read())
                            {
                                FlashcardDTO flashcard = new FlashcardDTO();
                                flashcard.Id = displayOrder;
                                flashcard.Front = reader.GetString(2);
                                flashcard.Back = reader.GetString(3);
                                flashcards.Add(flashcard);
                                displayOrder++;
                            }
                        }

                    }
                        break;
                default:
                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command = sqlConnection.CreateCommand();
                        command.CommandText = @"SELECT * FROM Flashcards_Table WHERE Stacks_Id = @Id";
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteReader();
                        using(var reader = command.ExecuteReader())
                        {
                            int displayOrder = 1;
                            while(reader.Read())
                            {
                                FlashcardDTO flashcard = new FlashcardDTO();
                                flashcard.Id = displayOrder;
                                flashcard.Front = reader.GetString(2);
                                flashcard.Back = reader.GetString(3);
                                flashcards.Add(flashcard);
                                displayOrder++;
                            }
                        }
                    }
                    break;
            }

            return flashcards;
        }
        public static Flashcard GetFlashcard(int Id)
        {
            Flashcard flashcard = new Flashcard();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"SELECT * FROM Flashcards_Table WHERE Stacks_Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteReader();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flashcard.Id = reader.GetInt32(0);
                        flashcard.Stacks_Id = reader.GetInt32(1);
                        flashcard.Front = reader.GetString(2);
                        flashcard.Back = reader.GetString(3);
                    }
                }
            }
            return flashcard;
        }
    }
}
