using ConsoleTableExt;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards
{
    internal class Flashcards
    {
        internal static void ViewAllFlashcards(string stackname)
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;
            List<FlashcardDTO> flashcards = new List<FlashcardDTO>();

            cnn = new SqlConnection(connectionString);
            cnn.Open();
            sql = $"SELECT f.Id, f.Question, f.Answer, s.Name " +
                $"FROM Flashcards f " +
                $"JOIN Stacks s ON f.StackId = s.Id " +
                $"WHERE CONVERT(VARCHAR, s.Name) = '{stackname}'";

            command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                flashcards.Add(new FlashcardDTO
                {
                    Id = reader.GetInt32(0),
                    Question = reader.GetString(1),
                    Answer = reader.GetString(2),
                    StackName = reader.GetString(3)
                });
            }
            cnn.Close();
            //remove gaps and clean IDs
            for (int i = 0; i < flashcards.Count; i++)
            {
                flashcards[i].Id = i + 1;
            }

            var listOfFlashcards = new List<List<object>>();

            foreach (FlashcardDTO flashcard in flashcards)
            {
                listOfFlashcards.Add(new List<object> { flashcard.Id, flashcard.Question, flashcard.Answer });
            }
            ConsoleTableBuilder
                .From(listOfFlashcards)
                .ExportAndWriteLine();


            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
        internal static void CreateNewFlashcard(string question, string answer, string stackname)
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            if (Helpers.DoesStacknameExist(stackname) == false)
            {
                Console.WriteLine("You need to create the stack before adding a flashcard to it!");
                Console.ReadLine();
            }
            else
            {
                sql = $"SELECT DISTINCT s.Id FROM Stacks s LEFT JOIN Flashcards f ON f.StackId = s.Id WHERE CONVERT(VARCHAR, s.Name) = '{stackname}'";
                command = new SqlCommand(sql, cnn);
                int stackId = (int)command.ExecuteScalar();

                sql = $"INSERT INTO Flashcards (Question, Answer, Stackname, StackId) " +
                    $"VALUES ('{question}', '{answer}', '{stackname}', '{stackId}')";
                command = new SqlCommand(sql, cnn);
                int rowsAdded = command.ExecuteNonQuery();
                Console.WriteLine($"\nYou added {rowsAdded} rows.");
            }

            cnn.Close();

            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
        internal static void DeleteFlashCard(int flashcardId)
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            sql = $"DELETE FROM Flashcards " +
                $"WHERE Id = {flashcardId}";

            command = new SqlCommand(sql, cnn);
            int rowsEdited = command.ExecuteNonQuery();

            Console.WriteLine($"You deleted {rowsEdited} rows.");

            cnn.Close();

            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
        internal static void UpdateFlashCard(int flashcardId, string newQuestion, string newAnswer, string newStackname)
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            if (Helpers.DoesStacknameExist(newStackname) == false)
            {
                Console.WriteLine("You need to create the stack before adding a flashcard to it!");
                Console.ReadLine();
            }
            else
            {
                sql = $"UPDATE Flashcards " +
                $"SET Question = '{newQuestion}', Answer = '{newAnswer}', Stackname = '{newStackname}' " +
                $"WHERE Id = {flashcardId}";

                command = new SqlCommand(sql, cnn);
                int rowsEdited = command.ExecuteNonQuery();
                Console.WriteLine($"You updated {rowsEdited} rows.");
            }

            cnn.Close();

            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
    }
}
