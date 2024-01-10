using Flashcards.StanimalTheMan.Models;
using System.Data.SqlClient;

namespace Flashcards.StanimalTheMan;

internal class StacksInterface
{
    internal static void ShowMenu()
    {
        string connectionString = "Data Source=(LocalDb)\\LocalDBDemo;Initial Catalog=Flashcards;Integrated Security=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            List<Stack> stacks = new List<Stack>();
            try
            {
                connection.Open();

                // Perform database operations here

                //Console.WriteLine("Connection successful!");

                string selectFlashcardsQuery = $"SELECT * FROM Stacks";

                using (SqlCommand command = new SqlCommand(selectFlashcardsQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Flashcards:");

                        while (reader.Read())
                        {
                            int stackId = reader.GetInt32(0);
                            string stackName = reader.GetString(1);

                            stacks.Add(new Stack(stackId, stackName));
                        }

                        foreach (Stack stack in stacks)
                        {
                            Console.WriteLine(stack.StackName);
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
    }
}
