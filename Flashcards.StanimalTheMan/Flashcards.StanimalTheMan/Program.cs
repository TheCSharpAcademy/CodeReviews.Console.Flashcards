using Flashcards.StanimalTheMan;

class Program
{
    static void Main()
    {
        StartApp();
    }

    private static void StartApp()
    {
        DatabaseHelper.InitializeDatabase();
        MainMenu.ShowMenu();
    }
}