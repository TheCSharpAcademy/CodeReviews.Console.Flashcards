


using Flashcards.Bina28.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards.Bina28.DBmanager;

internal class FlashcardsDB
{
	private string connectionString;

	public FlashcardsDB()
	{
		// Retrieve the connection string from App.config
		connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
	}
	public List<FlashCardsDto> GetAllRecords(string stackName)
	{
		var cards = new List<FlashCardsDto>();

		try
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				string sql = @"
                    SELECT flashcard_id, question, answer 
                    FROM flashcards 
                    WHERE stack_id = (SELECT stack_id FROM stacks WHERE name = @stackName);
                ";

				using (var command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@stackName", stackName);

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{

							var flashCard = new FlashCardsDto(
							Convert.ToInt32(reader["flashcard_id"]),
							reader["question"].ToString(),
							reader["answer"].ToString()
						);
							cards.Add(flashCard);
						}
					}
				}
			}
		}
		catch (SqlException ex)
		{
			Console.WriteLine("SQL Error: " + ex.Message);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error: " + ex.Message);
		}

		return cards; // Return an empty list if an error occurred
	}


	internal void CreateCard(string? question, string? answer, string stackName)
	{
		string query = "INSERT INTO flashcards (question, answer, stack_id) VALUES (@question, @answer, (SELECT stack_id FROM stacks WHERE name = @stackName))";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@question", question);
			command.Parameters.AddWithValue("@answer", answer);
			command.Parameters.AddWithValue("@stackName", stackName);
			try
			{
				connection.Open();
				int result = command.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");

			}
		}
	}

	internal void DeleteCard(int number)
	{
		string query = "DELETE FROM flashcards  WHERE flashcard_id=@number";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@number", number);
			try
			{
				connection.Open();
				int result = command.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");

			}
		}
	}

	internal void UpdateCard(string? question, string? answer, int flashcardId, string stackName)
	{
		// Query to update the flashcard with the new question and answer
		string query = "UPDATE flashcards SET question = @question, answer = @answer, stack_id = (SELECT stack_id FROM stacks WHERE name = @stackName) WHERE flashcard_id = @flashcardId";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@question", question);
			command.Parameters.AddWithValue("@answer", answer);
			command.Parameters.AddWithValue("@flashcardId", flashcardId); // Use the actual flashcard_id
			command.Parameters.AddWithValue("@stackName", stackName);

			try
			{
				connection.Open();
				int result = command.ExecuteNonQuery();
				if (result > 0)
				{
					Console.WriteLine("Flashcard updated successfully.");
				}
				else
				{
					Console.WriteLine("No flashcard was updated. Please check if the flashcard ID is correct.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
	}

	public void CreateFlashcardTable()
	{
		string createTableQuery = @"
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'flashcards')
    BEGIN
        CREATE TABLE flashcards (
            flashcard_id BIGINT IDENTITY(1,1) PRIMARY KEY, 
            stack_id BIGINT NOT NULL, 
            question NVARCHAR(255) NOT NULL, 
            answer NVARCHAR(255) NOT NULL, 
            CONSTRAINT FK_flashcard_stack FOREIGN KEY (stack_id) REFERENCES stacks(stack_id) ON DELETE CASCADE,
            CONSTRAINT UQ_flashcard UNIQUE (stack_id, question, answer)  -- Ensure no duplicates
        );
    END";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand(createTableQuery, connection))
				{
					command.ExecuteNonQuery();

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while creating the table: {ex.Message}");
			}
		}
	}

	public void InsertVocabularyFlashcards()
	{
		var flashcards = new List<FlashcardsModel>
	{
		new FlashcardsModel(1, "katt", "cat"),
		new FlashcardsModel(1, "hund", "dog"),
		new FlashcardsModel(1, "bok", "book"),
		new FlashcardsModel(1, "hus", "house"),
		new FlashcardsModel(1, "bil", "car"),
		new FlashcardsModel(2, "gato", "cat"),
		new FlashcardsModel(2, "perro", "dog"),
		new FlashcardsModel(2, "libro", "book"),
		new FlashcardsModel(2, "casa", "house"),
		new FlashcardsModel(2, "coche", "car"),
		new FlashcardsModel(3, "katze", "cat"),
		new FlashcardsModel(3, "hund", "dog"),
		new FlashcardsModel(3, "buch", "book"),
		new FlashcardsModel(3, "haus", "house"),
		new FlashcardsModel(3, "auto", "car"),
		new FlashcardsModel(4, "chat", "cat"),
		new FlashcardsModel(4, "chien", "dog"),
		new FlashcardsModel(4, "livre", "book"),
		new FlashcardsModel(4, "maison", "house"),
		new FlashcardsModel(4, "voiture", "car")
	};

		string insertQuery = @"
    IF NOT EXISTS (SELECT 1 FROM flashcards WHERE stack_id = @stack_id AND question = @question AND answer = @answer)
    BEGIN
        INSERT INTO flashcards (stack_id, question, answer) 
        VALUES (@stack_id, @question, @answer);
    END";

		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();

				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					foreach (var flashcard in flashcards)
					{
						using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
						{
							command.Parameters.AddWithValue("@stack_id", flashcard.Stack_id);
							command.Parameters.AddWithValue("@question", flashcard.Question);
							command.Parameters.AddWithValue("@answer", flashcard.Answer);
							command.ExecuteNonQuery();
						}
					}

					transaction.Commit();
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while inserting flashcards: {ex.Message}");
			}
		}
	}

}

