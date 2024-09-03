namespace Flashcards;
public static class Application
{
    public static void Run()
    {
        MainMenuHandler mainMenuHandler = new();
        mainMenuHandler.DisplayMenu();
    }
}