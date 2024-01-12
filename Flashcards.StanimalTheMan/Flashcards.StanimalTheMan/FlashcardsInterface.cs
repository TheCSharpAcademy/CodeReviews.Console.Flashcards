using Flashcards.StanimalTheMan.DTOs;
using Flashcards.StanimalTheMan.Models;
using Spectre.Console;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Flashcards.StanimalTheMan;

internal enum ManageFlashcardsOption
{
    ReturnToMainMenu,
    ChangeCurrentStack,
    ViewAllFlashcardsInStack,
    ViewXAmountOfCardsInStack,
    CreateAFlashcardInCurrentStack,
    EditFlashcard,
    DeleteFlashcard
}

internal class FlashcardsInterface
{
    internal static void ShowMenu()
    {
        string connectionString = "Data Source=(LocalDb)\\LocalDBDemo;Initial Catalog=Flashcards;Integrated Security=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            List<Stack> stacks = new List<Stack>();
            List<string> stackSelectOptions = new List<string>();
            stackSelectOptions.Add("Return to Main Menu");
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

                        while (reader.Read())
                        {
                            int stackId = reader.GetInt32(0);
                            string stackName = reader.GetString(1);

                            stacks.Add(new Stack(stackId, stackName));
                        }

                        foreach (Stack stack in stacks)
                        {
                            stackSelectOptions.Add(stack.StackName);
                        }

                        Console.WriteLine("Input a current stack name or return to Main Menu");
                        var selection = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Name")
                            .PageSize(10)
                            .AddChoices(stackSelectOptions));

                        Console.Clear();
                        if (selection == "Return to Main Menu")
                        {
                            MainMenu.ShowMenu();
                        }

                        ShowManageFlashcardsMenu(selection);
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

    private static void ShowManageFlashcardsMenu(string workingStack)
    {
        Console.WriteLine("---------------------------------");
        Console.WriteLine($"Current working stack: {workingStack}");

        // probably bad that I'm making a query for the stack which was already found in calling environment above
        string connectionString = "Data Source=(LocalDb)\\LocalDBDemo;Initial Catalog=Flashcards;Integrated Security=True";
        int selectedStackId = 0;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Perform database operations here

                //Console.WriteLine("Connection successful!");

                string selectFlashcardsQuery = $"SELECT * FROM Stacks WHERE StackName = @StackName";
                
                using (SqlCommand command = new SqlCommand(selectFlashcardsQuery, connection))
                {
                    command.Parameters.AddWithValue("@StackName", workingStack);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int stackId = reader.GetInt32(0);

                            selectedStackId = stackId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<ManageFlashcardsOption>()
                    .Title("")
                    .PageSize(10)
                    .AddChoices(ManageFlashcardsOption.ReturnToMainMenu, ManageFlashcardsOption.ChangeCurrentStack, ManageFlashcardsOption.ViewAllFlashcardsInStack, ManageFlashcardsOption.ViewXAmountOfCardsInStack, ManageFlashcardsOption.CreateAFlashcardInCurrentStack, ManageFlashcardsOption.EditFlashcard, ManageFlashcardsOption.DeleteFlashcard));

        switch (selection)
        {
            case ManageFlashcardsOption.ReturnToMainMenu:
                MainMenu.ShowMenu();
                break;
            case ManageFlashcardsOption.ChangeCurrentStack:
                ShowMenu();
                break;
            case ManageFlashcardsOption.ViewAllFlashcardsInStack:
                ViewAllFlashcardsInStack(selectedStackId);
                break;
            //case ManageFlashcardsOption.ViewXAmountOfCardsInStack:
            //    ViewXAmountOfCardsInStack();
            //    break;
            case ManageFlashcardsOption.CreateAFlashcardInCurrentStack:
                CreateAFlashcardInCurrentStack(selectedStackId);
                break;
                //case ManageFlashcardsOption.EditFlashcard:
                //    EditFlashcard();
                //    break;
                //case ManageFlashcardsOption.DeleteFlashcard:
                //    DeleteFlashcard();
                //    break;
        }
    }

    private static void CreateAFlashcardInCurrentStack(int selectedStackId)
    {


        SqlConnection connection = null;

        try
        {
            connection = DatabaseHelper.GetOpenConnection();

            // Perform database operations here

            //Console.WriteLine("Connection successful!");
            Console.WriteLine("Enter text for the front of the new Flashcard");
            string front = Console.ReadLine();
            Console.WriteLine("Enter text for the back of the new Flashcard");
            string back = Console.ReadLine();

            string createFlashcardQuery = $"INSERT INTO Flashcards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)";

            using (SqlCommand command = new SqlCommand(createFlashcardQuery, connection))
            {
                List<FlashcardDTO> flashcardDTOs = new();
                command.Parameters.AddWithValue("@Front", front);
                command.Parameters.AddWithValue("@Back", back);
                command.Parameters.AddWithValue("@StackId", selectedStackId);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Flashcard created successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to create the flashcard.");
                }
                Console.Clear();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            DatabaseHelper.CloseConnection(connection);
        }
        
    }

    private static void ViewAllFlashcardsInStack(int selectedStackId)
    {


        SqlConnection connection = null;
        try
        {
            connection = DatabaseHelper.GetOpenConnection();

            // Perform database operations here

            //Console.WriteLine("Connection successful!");

            string selectFlashcardsQuery = $"SELECT * FROM Flashcards WHERE StackId = @SelectedStackId";

            using (SqlCommand command = new SqlCommand(selectFlashcardsQuery, connection))
            {
                List<FlashcardDTO> flashcardDTOs = new();
                command.Parameters.AddWithValue("@SelectedStackId", selectedStackId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int flashcardId = reader.GetInt32(0);
                        string front = reader.GetString(1);
                        string back = reader.GetString(2);
                        flashcardDTOs.Add(new FlashcardDTO(flashcardId, front, back));
                    }
                }
                foreach(FlashcardDTO flashcardDTO in flashcardDTOs)
                {
                    Console.WriteLine($"{flashcardDTO.FlashcardId} {flashcardDTO.Front} {flashcardDTO.Back}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            DatabaseHelper.CloseConnection(connection);
        }

    }
}
