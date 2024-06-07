using System.Collections;

namespace Flashcards.UndercoverDev.UserInteraction
{
    public interface IUserConsole
    {
        string GetUserInput(string message);
        string MainMenu();
        string DeleteStackMenu(List<string> stacks);
        Task PrintMessage(string message, string color);
    }
}