using Flashcards.Model;
using Flashcards.View;
using Microsoft.Data.SqlClient;

namespace Flashcards.Controller
{
    public class StacksController
    {
        public static void AddStack()
        {
            Display.PrintAllStacks("Add Stack");

            string name = UI.PromptForAlphaNumericInput("\nEnter a name for the new stack: ", false, true);

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO Stacks (StackName)
                    VALUES (@name)";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = insertQuery;
                    command.Parameters.AddWithValue("@name", name);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllStacks("Add Stack");
                        Console.WriteLine("\nStack added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to add stack. Please try again.");
                    }
                }
            }
        }

        public static void EditStack()
        {
            var (stack, index) = Display.PrintStackSelectionMenu("Edit Stack", "Select a stack to edit...");

            int stackId = stack.Id;

            int indexPlusOne = index + 1;

            Display.PrintAllStacks("Edit Stack");

            string? currentStackName = null;

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string stackQuery = "SELECT * FROM Stacks WHERE StackId = @stackId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = stackQuery;
                    command.Parameters.AddWithValue("@stackId", stackId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentStackName = reader["StackName"].ToString();
                        }
                    }
                }

                Console.WriteLine($"\nSelected stack ID: {indexPlusOne}");
                Console.WriteLine($"Stack Name: {currentStackName}");

                string newStackName = UI.PromptForAlphaNumericInput($"\nEnter new stack name (leave blank to keep current): ", true); 
                
                if (string.IsNullOrEmpty(newStackName))
                {
                    newStackName = currentStackName!;
                }

                if (newStackName == currentStackName)
                {
                    Console.WriteLine("\nNo changes were made.");
                    return;
                }

                string updateStackQuery = "UPDATE Stacks SET StackName = @newStackName WHERE StackId = @stackId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = updateStackQuery;
                    command.Parameters.AddWithValue("@stackId", stackId);
                    command.Parameters.AddWithValue("@newStackName", newStackName);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllStacks("Edit Stack");
                        Console.WriteLine("\nStack updated successfully!");
                    }
                }
            }

        }

        public static void DeleteStack()
        {
            var (stack, index) = Display.PrintStackSelectionMenu("Delete Stack", "Select a stack to delete...");

            int stackId = stack.Id;

            int indexPlusOne = index + 1;

            Display.PrintAllStacks("Delete Stack");

            if (UI.PromptForDeleteConfirmation(indexPlusOne, "stack") == "n")
            {
                return;
            }

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string deleteQuery = @"
                    DELETE FROM Stacks
                    WHERE StackId = @stackId";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = deleteQuery;
                    command.Parameters.AddWithValue("@stackId", stackId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Display.PrintAllStacks("Delete Stack");
                        Console.WriteLine("\nStack deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nNo stack found with that ID. Deletion failed.");
                    }
                }
            }
        }
    }
}
