using Flashcards.selnoom.Data;
using Microsoft.Data.SqlClient;

namespace Flashcards.selnoom.Helpers;

internal class Helpers
{
    internal void CreateFlashcard(FlashcardRepository flashcardRepository, int stackId, string question, string answer)
    {
        try
        {
            flashcardRepository.CreateFlashcard(stackId, question, answer);
            Console.WriteLine("\nFlashcard created successfully! Press enter to continue");
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            Console.WriteLine("\nError: A flashcard with that question already exists in this stack. Please use a different question.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
        }
    }
}
