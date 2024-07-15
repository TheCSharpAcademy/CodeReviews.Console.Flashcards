using flashcards.Controllers;
using flashcards.Repositories;

class Program
{
    static void Main(string[] args)
    {
        var databaseManager = new DatabaseManager();

        try
        {
            databaseManager.CreateTables();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating tables: {ex.Message}");
        }

        // insert flashcards data for tests
        // var insertData = new InsertData();
        // insertData.InsertFlashcardsData();

        new GetUserInput().MainMenu();

    }
}
