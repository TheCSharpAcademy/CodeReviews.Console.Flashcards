using Flashcards.Models;

namespace Flashcards.Data;

public class SeedData
{
    internal static void SeedFlashcards()
    {
        List<Category> categories = new()
        {
            new Category { Name = "Math" },
            new Category { Name = "Science" },
            new Category { Name = "Technology" },
        };

        List<Flashcard> flashcards = new()
        {
            new Flashcard { CategoryId = 1, Question = "5 + 7 + 15", Answer = "27" },
            new Flashcard { CategoryId = 1, Question = "23 + 18 + 7", Answer = "48" },
            new Flashcard { CategoryId = 1, Question = "92 + 16 - 42", Answer = "66" },
            new Flashcard { CategoryId = 2, Question = "How many bones are in the human body?", Answer = "206" },
            new Flashcard
                { CategoryId = 2, Question = "What is the hardest natural substance on Earth? ", Answer = "Diamond" },
            new Flashcard
                { CategoryId = 2, Question = "At what temperature are Celsius and Fahrenheit equal?", Answer = "-40" },
            new Flashcard { CategoryId = 3, Question = "What is the best programming language?", Answer = "C#" },
            new Flashcard
            {
                CategoryId = 3, Question = "What does the 'IP' in IP address stand for?", Answer = "Internet Protocol"
            },
            new Flashcard
            {
                CategoryId = 3, Question = "What is the unit used to measure the processing speed of a computer?",
                Answer = "GHz"
            },
        };

        DataConnection dataConnection = new DataConnection();
        dataConnection.DeleteTables();
        dataConnection.CreateDatabase();
        dataConnection.InsertSeedSessions(categories, flashcards);
    }
}