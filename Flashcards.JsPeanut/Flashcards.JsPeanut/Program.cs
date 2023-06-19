using Flashcards.JsPeanut;
using System.Data.SqlClient;
class Program
{
    static string connectionString = "Data Source=(localdb)\\LocalDBDemo;Initial Catalog=Flashcards;Integrated Security=True;Connect Timeout=30;Encrypt=False";
    public static void Main(string[] args)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("IF OBJECT_ID('stacks') IS NULL CREATE TABLE stacks(stack_id INT IDENTITY(1,1) PRIMARY KEY, stack_name VARCHAR(255))", connection))
            {
                command.ExecuteNonQuery();
            }
            using (SqlCommand command = new SqlCommand("IF OBJECT_ID('flashcards') IS NULL CREATE TABLE flashcards(flashcard_id INT IDENTITY(1,1) PRIMARY KEY, flashcard_question VARCHAR(255), flashcard_answer VARCHAR(255), difficulty INT, stack_id INT, FOREIGN KEY(stack_id) REFERENCES stacks(stack_id))", connection))
            {
                command.ExecuteNonQuery();
            }
            using (SqlCommand command = new SqlCommand("IF OBJECT_ID('study_sessions') IS NULL CREATE TABLE study_sessions(Id INT IDENTITY(1,1) PRIMARY KEY, Hits INTEGER, Misses INTEGER, Score INTEGER, Month INTEGER, stack_id INTEGER, stack_name VARCHAR(255), FOREIGN KEY (stack_id) REFERENCES stacks(stack_id))", connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        UserInput.GetUserInput();
    }
}