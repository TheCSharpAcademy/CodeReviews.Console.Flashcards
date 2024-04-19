using DbHelpers.HopelessCoding;
using FlashCards.HopelessCoding.DTOs;
using HelperMethods.HopelessCoding;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flashcards.HopelessCoding;

internal class FlashcardCommands
{
    internal static void CreateNewFlashcard(string stackName)
    {
        string createStackQuery = "INSERT INTO Flashcards " +
                                  "VALUES (@StackName, @Front, @Back);";

        Console.Write("Please enter the text for the front side of the flashcard: ");
        var frontSide = Console.ReadLine();

        Console.Write("Please enter the text for the back side of the flashcard: ");
        var backSide = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(DatabaseHelpers.connectionString))
        {
            try
            {
                connection.Open();

                SqlCommand createStackCommand = new SqlCommand(createStackQuery, connection);
                createStackCommand.Parameters.AddWithValue("@StackName", stackName);
                createStackCommand.Parameters.AddWithValue("@Front", frontSide);
                createStackCommand.Parameters.AddWithValue("@Back", backSide);
                createStackCommand.ExecuteNonQuery();

                AnsiConsole.Write(new Markup("[green]\nFlashcard added successfully.[/]\n\n"));
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"[red]$An error occurred: {ex.Message}[/]"));
            }
        }
        Helpers.WaitForUserInput();
    }

    internal static void DeleteFlashcard(string stackName)
    {
        ViewFlashcards(stackName, null);

        int idToDelete = Helpers.GetValidIdFromUser(stackName, "delete");
        if (idToDelete == -1)
        {
            return;
        }

        string deleteFlashcardQuery = "DELETE FROM Flashcards " +
                                      "WHERE Id = @Id;";

        using (SqlConnection connection = new SqlConnection(DatabaseHelpers.connectionString))
        {
            try
            {
                connection.Open();

                SqlCommand createStackCommand = new SqlCommand(deleteFlashcardQuery, connection);
                createStackCommand.Parameters.AddWithValue("@Id", idToDelete);
                createStackCommand.ExecuteNonQuery();

                AnsiConsole.Write(new Markup("[green]\nFlashcard deleted successfully.[/]\n\n"));
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"[red]$An error occurred: {ex.Message}[/]"));
            }
        }
        Helpers.WaitForUserInput();
    }

    internal static void EditFlashcard(string stackName)
    {
        ViewFlashcards(stackName, null);

        int idToUpdate = Helpers.GetValidIdFromUser(stackName, "update");
        if (idToUpdate == -1)
        {
            return;
        }

        string editFlashcardQuery = "UPDATE Flashcards " +
                                    "SET Front = @newFront, Back = @newBack " +
                                    "WHERE Id = @Id;";

        Console.Write("Please enter the new text for the front side of the flashcard: ");
        var newFront = Console.ReadLine();

        Console.Write("Please enter the new text for the back side of the flashcard: ");
        var newBack = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(DatabaseHelpers.connectionString))
        {
            try
            {
                connection.Open();

                SqlCommand createStackCommand = new SqlCommand(editFlashcardQuery, connection);
                createStackCommand.Parameters.AddWithValue("@Id", idToUpdate);
                createStackCommand.Parameters.AddWithValue("@newFront", newFront);
                createStackCommand.Parameters.AddWithValue("@newBack", newBack);
                createStackCommand.ExecuteNonQuery();

                AnsiConsole.Write(new Markup("[green]\nFlashcard updated successfully.[/]\n\n"));
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"[red]$An error occurred: {ex.Message}[/]"));
            }
        }
        Helpers.WaitForUserInput();
    }

    internal static void ViewFlashcards(string stackName, int? amount)
    {
        FlashcardService flashcardService = new FlashcardService(DatabaseHelpers.connectionString);
        flashcardService.PrintFlashcards(stackName, amount);
    }
}