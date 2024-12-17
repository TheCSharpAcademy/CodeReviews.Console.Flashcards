


using Flashcards.Bina28.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.Bina28.DBmanager
{
	internal class FlashcardsDB
	{
		private string connectionString;

		public FlashcardsDB()
		{
			// Retrieve the connection string from App.config
			connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
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
                        FROM Flashcards 
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
			string query = "INSERT INTO Flashcards (question, answer, stack_id) VALUES (@question, @answer, (SELECT stack_id FROM Stacks WHERE name = @stackName))";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@question", question);
				command.Parameters.AddWithValue("@answer", answer);
				command.Parameters.AddWithValue("@stackName", stackName);

				try
				{
					connection.Open();
					command.ExecuteNonQuery();
					Console.WriteLine($"The question: {question} and answer: {answer} were successfully added.");
				}
				catch (SqlException ex)
				{
					Console.WriteLine($"SQL Error: {ex.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred: {ex.Message}");
				}
			}
		}

		internal void DeleteCard(int number)
		{
			string query = "DELETE FROM Flashcards WHERE flashcard_id=@number";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@number", number);

				try
				{
					connection.Open();
					int result = command.ExecuteNonQuery();
					if (result > 0)
					{
						Console.WriteLine($"Flashcard was deleted successfully.");
					}
					else
					{
						Console.WriteLine($"No flashcard found with ID {number}.");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred: {ex.Message}");
				}
			}
		}

		internal void UpdateCard(string? question, string? answer, int flashcardId, string stackName)
		{
			string query = "UPDATE Flashcards SET question = @question, answer = @answer, stack_id = (SELECT stack_id FROM stacks WHERE name = @stackName) WHERE flashcard_id = @flashcardId";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@question", question);
				command.Parameters.AddWithValue("@answer", answer);
				command.Parameters.AddWithValue("@flashcardId", flashcardId);
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
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Flashcards')
BEGIN
    CREATE TABLE Flashcards (
        flashcard_id INT IDENTITY(1,1) PRIMARY KEY, 
        stack_id INT NOT NULL, 
        question NVARCHAR(255) NOT NULL, 
        answer NVARCHAR(255) NOT NULL, 

        CONSTRAINT FK_flashcard_stack FOREIGN KEY (stack_id) REFERENCES Stacks(stack_id) ON DELETE CASCADE,
        CONSTRAINT UQ_flashcard UNIQUE (stack_id, question, answer)
    );
END
";

			// Query to find the highest flashcard_id
			string findMaxFlashcardIdQuery = "SELECT ISNULL(MAX(flashcard_id), 0) FROM Flashcards";

			// Query to reseed the identity column
			string reseedQuery = "DBCC CHECKIDENT ('Flashcards', RESEED, @maxFlashcardId)";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					connection.Open();

					// Create the table if it doesn't exist
					using (SqlCommand createCommand = new SqlCommand(createTableQuery, connection))
					{
						createCommand.ExecuteNonQuery();
						
					}

					// Find the highest flashcard_id
					int maxFlashcardId;
					using (SqlCommand findMaxFlashcardIdCommand = new SqlCommand(findMaxFlashcardIdQuery, connection))
					{
						maxFlashcardId = (int)findMaxFlashcardIdCommand.ExecuteScalar();
					}

					// Reseed the identity column based on the max flashcard_id
					using (SqlCommand reseedCommand = new SqlCommand(reseedQuery, connection))
					{
						// Add the @maxFlashcardId parameter
						reseedCommand.Parameters.AddWithValue("@maxFlashcardId", maxFlashcardId);
						reseedCommand.ExecuteNonQuery();
					
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("An error occurred: " + ex.Message);
				}
			}
		}

		OperationManager operationManager = new();

		public void InsertVocabularyFlashcards()
		{
			var flashcards = new List<FlashcardsModel>
	{
		new FlashcardsModel("Norwegian", "katt", "cat"),
		new FlashcardsModel("Norwegian", "hund", "dog"),
		new FlashcardsModel("Norwegian", "bok", "book"),
		new FlashcardsModel("Spanish", "gato", "cat"),
		new FlashcardsModel("Spanish", "perro", "dog"),
		new FlashcardsModel("German", "katze", "cat"),
		new FlashcardsModel("German", "hund", "dog"),
		new FlashcardsModel("English", "cat", "cat"),
		new FlashcardsModel("English", "dog", "dog")
	};

			if (!operationManager.IsOperationComplete("Flashcard"))
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					try
					{
						connection.Open();

						foreach (var flashcard in flashcards)
						{
							// Retrieve the stack_id using the stack name (navn) from the Stacks table
							string getStackIdQuery = "SELECT stack_id FROM Stacks WHERE name = @name";
							int stackId = -1; // Default to -1 if no stack is found

							using (var getStackIdCommand = new SqlCommand(getStackIdQuery, connection))
							{
								getStackIdCommand.Parameters.AddWithValue("@name", flashcard.Name); // Using "Name" from model (which is "navn")
								var result = getStackIdCommand.ExecuteScalar();

								if (result != null)
								{
									stackId = Convert.ToInt32(result); // Retrieve the stack_id
								}
							}

							// Now insert the flashcard using the retrieved stack_id
							if (stackId != -1) // Ensure the stack_id is valid
							{
								string insertFlashcardQuery = "INSERT INTO Flashcards (stack_id, question, answer) VALUES (@stack_id, @question, @answer)";
								using (var insertFlashcardCommand = new SqlCommand(insertFlashcardQuery, connection))
								{
									insertFlashcardCommand.Parameters.AddWithValue("@stack_id", stackId);
									insertFlashcardCommand.Parameters.AddWithValue("@question", flashcard.Question);
									insertFlashcardCommand.Parameters.AddWithValue("@answer", flashcard.Answer);
									insertFlashcardCommand.ExecuteNonQuery();
								}
							}
							else
							{
								Console.WriteLine($"Stack '{flashcard.Name}' not found, skipping flashcard insertion.");
							}
						}

						operationManager.MarkOperationComplete("Flashcard");
					}
					catch (Exception ex)
					{
						Console.WriteLine($"An error occurred while processing flashcards: {ex.Message}");
					}
				}
			}
		}


	}
}
