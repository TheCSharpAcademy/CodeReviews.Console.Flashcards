using Microsoft.Data.SqlClient;

namespace Flashcards.Model
{
    public class FlashcardsRepository
    {
        private readonly string _connectionString;

        public FlashcardsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateTable()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Flashcards')
                    BEGIN
                        CREATE TABLE Flashcards(
                            FlashcardId INT IDENTITY(1,1) PRIMARY KEY,
                            StackId INT NOT NULL,
                            Question TEXT NOT NULL,
                            Answer TEXT NOT NULL,
                            FOREIGN KEY (StackID) REFERENCES Stacks(StackId) ON DELETE CASCADE
                        );
                    END;";

                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"An error occurred while creating the Flashcards table: {ex.Message}");
                    }
                }
            }
        }

        public void SeedFlashcards()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string insertFlashcardsQuery = "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId);";

                var flashcards = new List<(string Question, string Answer, int StackId)>
                {
                    ("2 + 2", "4", 1),
                    ("5 * 4", "20", 1),
                    ("30 / 2", "15", 1),
                    ("57 - 12", "45", 1),
                    ("What is the chemical symbol for water?", "H20", 2),
                    ("How many bones are in the adult human body?", "206", 2),
                    ("What force pulls objects toward Earth?", "gravity", 2),
                    ("What is the process by which plants make their food?", "photosynthesis", 2),
                    ("Who was the first president of the USA?", "George Washington", 3),
                    ("In what year did World War II end?", "1945", 3),
                    ("In what year did the Berlin Wall fall?", "1989", 3),
                    ("Who painted the Mona Lisa?", "Leonardo da Vinci", 3)
                };

                foreach (var (Question, Answer, StackId) in flashcards)
                {
                    using (SqlCommand command = new SqlCommand(insertFlashcardsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Question", Question);
                        command.Parameters.AddWithValue("@Answer", Answer);
                        command.Parameters.AddWithValue("@StackId", StackId);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<FlashcardDto> GetAllFlashcardsForStack(int stackId)
        {
            var flashcards = new List<FlashcardDto>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT FlashcardId, Question, Answer FROM Flashcards WHERE StackId = @stackId ORDER BY FlashcardId DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@stackId", stackId));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var flashcard = new FlashcardDto
                            {
                                FlashcardId = reader.GetInt32(0),
                                Question = reader.GetString(1),
                                Answer = reader.GetString(2)
                            };
                            flashcards.Add(flashcard);
                        }
                    }
                }
            }

            return flashcards;
        }
    }
}