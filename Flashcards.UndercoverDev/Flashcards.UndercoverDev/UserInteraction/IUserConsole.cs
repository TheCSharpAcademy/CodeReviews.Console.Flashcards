using Spectre.Console;

namespace Flashcards.UndercoverDev.UserInteraction
{
    public interface IUserConsole
    {
        string GetUserInput(string message);
        string MainMenu();
        void PrintMessage(string message, string color);
        void WaitForAnyKey();
        string ShowMenu(string message, List<string> list);
        void WritTable(Table table);
    }
}