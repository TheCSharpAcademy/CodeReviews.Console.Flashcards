namespace Flashcards;

public class Flashcards
{
    static void Main(string[] args)
    {
        var DataAccess = new DataAcess();

        DataAccess.CreateTables();

        UserInterface.MainMenu();
    }
}

