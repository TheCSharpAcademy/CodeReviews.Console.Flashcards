using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=(LocalDb)\\LocalDBDemo;Initial Catalog=Flashcards;Integrated Security=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Perform database operations here

                //Console.WriteLine("Connection successful!");

                string selectFlashcardsQuery = $"SELECT * FROM Flashcards WHERE StackId = {1}";

                using (SqlCommand command = new SqlCommand(selectFlashcardsQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Flashcards:");

                        while (reader.Read())
                        {
                            int flashcardId = reader.GetInt32(0);
                            string question = reader.GetString(1);
                            string answer = reader.GetString(2);

                            Console.WriteLine($"FlashcardId: {flashcardId}, Question: {question}, Answer: {answer}");
                        }
                    }
                }

                // Ensure to close the connection when done
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        Console.ReadLine();
    }
}