namespace Flashcards;

class Flashcards
{
    public static DataController ProgramController = new();
    public static void Main()
    {
           ProgramController.MainMenuController(); //Pending DB Init and DB populate, modify varchar to 250
    }
}
