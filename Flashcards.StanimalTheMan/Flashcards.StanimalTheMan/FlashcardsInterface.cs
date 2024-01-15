﻿using Flashcards.StanimalTheMan.DTOs;
using Flashcards.StanimalTheMan.Models;
using Spectre.Console;
using System.Data.SqlClient;

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
        SqlConnection connection = null;
        using (connection = DatabaseHelper.GetOpenConnection())
        {
            List<Stack> stacks = new List<Stack>();
            List<string> stackSelectOptions = new List<string>();
            stackSelectOptions.Add("Return to Main Menu");
            try
            {


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
        SqlConnection connection = null;

        int selectedStackId = 0;
        try
        {
            connection = DatabaseHelper.GetOpenConnection();

            string selectStackQuery = $"SELECT * FROM Stacks WHERE StackName = @StackName";

            using (SqlCommand command = new SqlCommand(selectStackQuery, connection))
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
        finally
        {
            DatabaseHelper.CloseConnection(connection);
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
                ViewAllFlashcardsInStack(selectedStackId, -1);
                break;
            case ManageFlashcardsOption.ViewXAmountOfCardsInStack:
                int numFlashcards = GetXAmount();
                ViewAllFlashcardsInStack(selectedStackId, numFlashcards);
                break;
            case ManageFlashcardsOption.CreateAFlashcardInCurrentStack:
                CreateAFlashcardInCurrentStack(selectedStackId);
                break;
            case ManageFlashcardsOption.EditFlashcard:
                EditFlashcard(selectedStackId);
                break;
            case ManageFlashcardsOption.DeleteFlashcard:
                DeleteFlashcard(selectedStackId);
                break;
        }
    }

    private static void EditFlashcard(int selectedStackId)
    {
        SqlConnection connection = null;

        try
        {
            connection = DatabaseHelper.GetOpenConnection();

            // Perform database operations here

            Console.WriteLine("Select the flashcard you want to update");

            List<FlashcardDto> flashcardDTOs = new();
            List<string> flashcardSelectionOptions = new();

            string selectFlashcardsQuery = $"SELECT FlashcardId, ROW_NUMBER() OVER (ORDER BY FlashcardId) AS SequentialId, Front, Back FROM Flashcards WHERE StackId = @StackId";

            SqlCommand getFlashcards = new SqlCommand(selectFlashcardsQuery, connection);
            try
            {
                // to map sequential id to flashcard id to update by flashcard id later
                Dictionary<long, int> mapping = new();
                getFlashcards.Parameters.AddWithValue("@StackId", selectedStackId);
                using (SqlDataReader reader = getFlashcards.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int flashcardId = reader.GetInt32(0);
                        long sequentialId = reader.GetInt64(1);
                        string front = reader.GetString(2);
                        string back = reader.GetString(3);

                        mapping.Add(sequentialId, flashcardId);
                        flashcardDTOs.Add(new FlashcardDto(sequentialId, flashcardId, front, back));
                    }
                }

                foreach (FlashcardDto flashcardDTO in flashcardDTOs)
                {
                    string selectionString = "";
                    selectionString += flashcardDTO.FlashcardSequentialId;
                    selectionString += $"\t{flashcardDTO.Front}";
                    selectionString += $"\t{flashcardDTO.Back}";
                    flashcardSelectionOptions.Add(selectionString);
                }

                flashcardSelectionOptions.Add("Return to Manage Stacks Menu");

                Console.WriteLine("Choose a flashcard to update");
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("-------------------------------")
                    .PageSize(10)
                    .AddChoices(flashcardSelectionOptions));

                if (selection == "Return to Manage Stacks Menu")
                {
                    Console.Clear();
                    ShowMenu();
                }

                Console.WriteLine("Enter new front of flashcard");
                string updatedFront = Console.ReadLine();

                Console.WriteLine("Enter new back of flashcard");
                string updatedBack = Console.ReadLine();

                // naively update both even if user enters same front and back...
                string updateFlashcardQuery = $"UPDATE flashcards SET Front = @UpdatedFront, Back = @UpdatedBack WHERE FlashcardId = @FlashcardId";
                using (SqlCommand updateFlashcardCommand = new SqlCommand(updateFlashcardQuery, connection))
                {
                    int flashcardId = mapping[Int32.Parse(selection.Split('\t')[0])];
                    updateFlashcardCommand.Parameters.AddWithValue("@FlashcardId", flashcardId);
                    updateFlashcardCommand.Parameters.AddWithValue("@UpdatedFront", updatedFront);
                    updateFlashcardCommand.Parameters.AddWithValue("@UpdatedBack", updatedBack);
                    int rowsAffected = updateFlashcardCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Flashcard {flashcardId} updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"No flashcard found with the name '{selection}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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

    private static void DeleteFlashcard(int selectedStackId)
    {
        SqlConnection connection = null;

        try
        {
            connection = DatabaseHelper.GetOpenConnection();
            Console.WriteLine("Select the flashcard you want to delete");

            Dictionary<long, int> mapping = new();
            List<FlashcardDto> flashcardDTOs = new();
            List<string> flashcardSelectionOptions = new();
            string fetchFlashcardsQuery = $"SELECT FlashcardId, ROW_NUMBER() OVER (ORDER BY FlashcardId) AS SequentialId, Front, Back FROM Flashcards WHERE StackId = @StackId";

            SqlCommand getFlashcards = new SqlCommand(fetchFlashcardsQuery, connection);
            try
            {
                getFlashcards.Parameters.AddWithValue("@StackId", selectedStackId);
                using (SqlDataReader reader = getFlashcards.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int flashcardId = reader.GetInt32(0);
                        long sequentialId = reader.GetInt64(1);
                        string front = reader.GetString(2);
                        string back = reader.GetString(3);

                        mapping.Add(sequentialId, flashcardId);
                        flashcardDTOs.Add(new FlashcardDto(sequentialId, flashcardId, front, back));
                    }
                }

                foreach (FlashcardDto flashcardDTO in flashcardDTOs)
                {
                    string selectionString = "";
                    selectionString += flashcardDTO.FlashcardSequentialId;
                    selectionString += $"\t{flashcardDTO.Front}";
                    selectionString += $"\t{flashcardDTO.Back}";
                    flashcardSelectionOptions.Add(selectionString);
                }

                flashcardSelectionOptions.Add("Return to Manage Flashcards Menu");

                Console.WriteLine("Choose a flashcard to delete or return to main menu");
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("-------------------------------")
                    .PageSize(10)
                    .AddChoices(flashcardSelectionOptions));

                if (selection == "Return to Manage Flashcards Menu")
                {
                    Console.Clear();
                    ShowMenu();
                }

                string deleteFlashcardQuery = $"DELETE FROM flashcards WHERE FlashcardId = @FlashcardId";
                using (SqlCommand deleteFlashcardCommand = new SqlCommand(deleteFlashcardQuery, connection))
                {
                    deleteFlashcardCommand.Parameters.AddWithValue("@FlashcardId", mapping[Int32.Parse(selection.Split('\t')[0])]);

                    int rowsAffected = deleteFlashcardCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Flashcard '{selection}' deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"No flashcard found with the name '{selection}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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

    private static int GetXAmount()
    {
        Console.WriteLine("Enter the number of flashcards you want to fetch from selected stack: ");
        int numFlashcards;
        while (!Int32.TryParse(Console.ReadLine(), out numFlashcards))
        {
            Console.WriteLine("Enter the number of flashcards you want to fetch from selected stack: ");
        }
        return numFlashcards;
    }

    private static void CreateAFlashcardInCurrentStack(int selectedStackId)
    {


        SqlConnection connection = null;

        try
        {
            connection = DatabaseHelper.GetOpenConnection();

            // Perform database operations here
            Console.WriteLine("Enter text for the front of the new Flashcard");
            string front = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(front))
            {
                Console.WriteLine("Front cannot be empty, null, or whitespace");
                front = Console.ReadLine();
            }
            Console.WriteLine("Enter text for the back of the new Flashcard");
            string back = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(back))
            {
                Console.WriteLine("Back cannot be empty, null, or whitespace");
                back = Console.ReadLine();
            }

            string createFlashcardQuery = $"INSERT INTO Flashcards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)";

            using (SqlCommand command = new SqlCommand(createFlashcardQuery, connection))
            {
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

    private static void ViewAllFlashcardsInStack(int selectedStackId, int limit)
    {


        SqlConnection connection = null;
        try
        {
            connection = DatabaseHelper.GetOpenConnection();

            string selectFlashcardsQuery = $"SELECT FlashcardId, ROW_NUMBER() OVER (ORDER BY FlashcardId) AS SequentialId, Front, Back FROM Flashcards WHERE StackId = @SelectedStackId";
            if (limit != -1)
            {
                // user wants to get only x number of flashcards
                selectFlashcardsQuery = $"SELECT TOP(@Limit) FlashcardId, ROW_NUMBER() OVER (ORDER BY FlashcardId) AS SequentialId, Front, Back FROM Flashcards WHERE StackId = @SelectedStackId";
            }

            using (SqlCommand command = new SqlCommand(selectFlashcardsQuery, connection))
            {
                List<FlashcardDto> flashcardDTOs = new();
                command.Parameters.AddWithValue("@SelectedStackId", selectedStackId);
                if (limit != -1)
                {
                    command.Parameters.AddWithValue("@Limit", limit);
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int flashcardId = reader.GetInt32(0);
                        long sequentialId = reader.GetInt64(1);
                        string front = reader.GetString(2);
                        string back = reader.GetString(3);
                        flashcardDTOs.Add(new FlashcardDto(sequentialId, flashcardId, front, back));
                    }
                }
                foreach (FlashcardDto flashcardDTO in flashcardDTOs)
                {
                    Console.WriteLine($"{flashcardDTO.FlashcardSequentialId} {flashcardDTO.Front} {flashcardDTO.Back}");
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
