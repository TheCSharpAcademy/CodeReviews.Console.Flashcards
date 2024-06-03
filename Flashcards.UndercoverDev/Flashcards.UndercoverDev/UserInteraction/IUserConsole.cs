namespace Flashcards.UndercoverDev.UserInteraction
{
    public interface IUserConsole
    {
        string GetUserInput(string message);
        string MainMenu();
        void PrintMessage(string message, string color);
    }
}