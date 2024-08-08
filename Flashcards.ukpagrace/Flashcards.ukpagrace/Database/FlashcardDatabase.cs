using Flashcards.ukpagrace.DTO;
using Flashcards.ukpagrace.Entity;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards.ukpagrace.Database
{
    class FlashCardDatabase
    {
        string connectionString = ConfigurationManager.AppSettings["ConfigurationString"] ?? string.Empty;
        public void CreateFlashcard()
        {
            try
            {
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"
                    IF NOT EXISTS(
                        SELECT * 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'flashcard' AND TABLE_SCHEMA = 'dbo'
                    )
                    BEGIN
                        CREATE TABLE dbo.flashcard(
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            StackId INT NOT NULL, 
                            Question VARCHAR(255) NOT NULL,
                            Answer VARCHAR(255) NOT NULL,
                            FOREIGN KEY(StackId)
                            REFERENCES dbo.stack(Id)
                            ON DELETE CASCADE ON UPDATE NO ACTION
                        );
                    END
                ";
                sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void Insert(int stackId, string question, string answer)
        {
            try
            {
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"INSERT INTO flashcard VALUES (@StackId, @Question, @Answer)";
                cmd.Parameters.AddWithValue("@StackId", stackId);
                cmd.Parameters.AddWithValue("@Question", question);
                cmd.Parameters.AddWithValue("@Answer", answer);
                sqlConnection.Open();
                cmd.ExecuteNonQuery();

                Console.WriteLine("inserted 1 record into stack");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Update(int id, int stackId, string question, string answer)
        {
            try
            {
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"
				UPDATE flashcard SET 
					StackId=@StackId, 
					Question=@Question,
					Answer=@Answer
				WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@StackId", stackId);
                cmd.Parameters.AddWithValue("@Question", question);
                cmd.Parameters.AddWithValue("@Answer", answer);
                sqlConnection.Open();
                var affectedRecords = cmd.ExecuteNonQuery();
                Console.WriteLine($"updated {affectedRecords} record into stack");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Delete(int stackId)
        {
            try
            {
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"DELETE FROM flashcard WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Id", stackId);
                sqlConnection.Open();
                var affectedRecords = cmd.ExecuteNonQuery();
                Console.WriteLine($"deleted {affectedRecords} record from flashcard");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public FlashcardEntity GetOne(int flashcardId)
        {
            try
            {
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM flashcard WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Id", flashcardId);
                sqlConnection.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var record = new FlashcardEntity
                    {
                        Id = reader.GetInt32(0),
                        StackId = reader.GetInt32(1),
                        Question = reader.GetString(2),
                        Answer = reader.GetString(3)
                    };
                    return record;
                }
                else
                {
                    throw new Exception("Flashcard not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<FlashcardDto> GetFlashcards(int stackId)
        {
            try
            {
                List<FlashcardDto> records = new List<FlashcardDto>();
                using SqlConnection sqlConnection = new(connectionString);
                using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM flashcard where @StackId=StackId";
                cmd.Parameters.AddWithValue("@StackId", stackId);
                sqlConnection.Open();

                SqlDataReader table = cmd.ExecuteReader();
                if (table.HasRows)
                {
                    while (table.Read())
                    {
                        records.Add(new FlashcardDto()
                        {
                            FlashcardId = table.GetInt32(0),
                            Question = table.GetString(2),
                            Answer = table.GetString(3),
                        });
                    }
                }
                return records;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}