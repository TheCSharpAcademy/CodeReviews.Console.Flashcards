namespace FlashCards.Doc415;

internal class Program
{
    static void Main(string[] args)
    {

        var DataAccess = new DataAccess();
        DataAccess.CreateTables();

        var userInterface = new UserInterface();
        userInterface.InitializeMenu();
    }
}
