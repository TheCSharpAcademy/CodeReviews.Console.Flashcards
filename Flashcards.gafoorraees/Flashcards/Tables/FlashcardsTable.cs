using System.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.Models;

namespace Flashcards.Tables;

public class FlashcardsTable
{
    private static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");

    public static void InsertFlashcard(string question, string answer, int stackId)
    {
        using var connection = new SqlConnection(connectionString);
        
        string getMaxDisplayIdQuery = "SELECT ISNULL(MAX(DisplayID), 0) + 1 FROM Flashcards WHERE StackID = @StackID";
        var displayId = connection.QuerySingle<int>(getMaxDisplayIdQuery, new { StackID = stackId });

        string insertQuery = @"INSERT INTO Flashcards (Question, Answer, StackID, DisplayID)
                                VALUES (@Question, @Answer, @StackID, @DisplayID)";

        var flashcardParameters = new
        {
            Question = question,
            Answer = answer,
            StackID = stackId,
            DisplayID = displayId
        };

        connection.Execute(insertQuery, flashcardParameters);
    }

    public static List<Flashcard> GetFlashcardsFromStack(int stackId)
    {
        using var connection = new SqlConnection(connectionString);
        
        string selectQuery = @"
            SELECT DisplayID, Question, Answer
            FROM Flashcards
            WHERE StackID = @StackID";

        var parameters = new { StackID = stackId };

        return connection.Query<Flashcard>(selectQuery, parameters).ToList();
    }

    public static void UpdateFlashcard(int stackID)
    {
        Console.Clear();

        var flashcards = FlashcardsTable.GetFlashcardsFromStack(stackID);

        if (flashcards.Count == 0)
        {
            Console.WriteLine("No flashcards available in this stack. Enter any key to return to Flashcard Management\n");
            Console.ReadLine();
            FlashcardUI.ManageFlashcards();
        }

        Console.WriteLine("Available Flashcards:\n");

        FlashcardUI.DisplayFlashcards(stackID);

        int flashcardID;
        while (true)
        {
            Console.WriteLine("Please enter the ID of the flashcard that you would like to update:\n");
            if (!int.TryParse(Console.ReadLine(), out flashcardID))
            {
                Console.WriteLine("Invalid input. Please enter a valid ID.");
                continue;
            }

            if (!flashcards.Any(f => f.DisplayID == flashcardID))
            {
                Console.WriteLine("Flashcard not found. Please try again");
                continue;
            }

            break;
        }
  
        var selectedFlashcard = flashcards.SingleOrDefault(f => f.DisplayID == flashcardID);

        Console.Clear();

        string question = FlashcardInput.GetQuestion("Please enter the updated question", true, selectedFlashcard.Question);
        string answer = FlashcardInput.GetAnswer("\nPlease enter the updated answer", true, selectedFlashcard.Answer);

        using var connection = new SqlConnection(connectionString);
        
        string updateSql = @"
            UPDATE Flashcards
            SET Question = @Question, Answer = @Answer
            WHERE DisplayID = @FlashcardID";

        connection.Execute(updateSql, new
        {
            Question = question,
            Answer = answer,
            FlashcardID = flashcardID
        });

        Console.Clear();

        Console.WriteLine("Flashcard updated successfully.");
    }

    public static void DeleteFlashcard(int stackId)
    {
        Console.Clear();

        var flashcards = FlashcardsTable.GetFlashcardsFromStack(stackId);

        if (flashcards.Count == 0)
        {
            Console.WriteLine("No flashcards in this stack to delete. Please add some.");
            return;
        }

        FlashcardUI.DisplayFlashcards(stackId);

        Console.WriteLine("\nPlease enter the ID of the flashcard that you want to remove:\n");
        var input = Console.ReadLine().Trim();

        if (int.TryParse(input, out int flashcardID))
        {
            using var connection = new SqlConnection(connectionString);
            
            string deleteQuery = "DELETE FROM Flashcards WHERE DisplayID = @DisplayID";
            var parameters = new { DisplayID = flashcardID };
            
            connection.Execute(deleteQuery, parameters);

            string reorderQuery = @"
                WITH OrderedFlashcards AS (
                    SELECT ID, ROW_NUMBER() OVER (ORDER BY ID) AS NewDisplayID
                    FROM Flashcards
                    WHERE StackID = @StackID
                )
                UPDATE Flashcards
                SET DisplayID = OrderedFlashcards.NewDisplayID
                FROM Flashcards
                JOIN OrderedFlashcards ON Flashcards.ID = OrderedFlashcards.ID";

            connection.Execute(reorderQuery, new { StackID = stackId });

            Console.Clear();

            Console.WriteLine("Flashcard deleted successfully.");
        }
        else
        {
            Console.WriteLine("Invalid ID. Please enter a valid integer.");
        }
    }
}

