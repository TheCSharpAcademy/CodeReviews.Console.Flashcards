using Flashcards.harris_andy;

internal class Program
{
    private static void Main(string[] args)
    {
        UserInput userInput = new UserInput();
        DisplayData displayData = new DisplayData();
        UseDB useDB = new UseDB();
        FlashCardController controller = new FlashCardController(displayData, userInput, useDB);

        controller.InitializeDatabase();
        controller.ShowMainMenu();
    }
}
