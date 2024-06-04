using Flashcards.Models;

namespace Flashcards;

internal class FlashcardsController
{
    internal string ConnectionString {get; init;}
    internal FlashcardsDbContext DbContext;

    public FlashcardsController(string connectionString)
    {
        ConnectionString = connectionString;
        DbContext = new FlashcardsDbContext(connectionString);
    }

    internal void RunApp()
    {
        Console.WriteLine("Start App");
    }
}