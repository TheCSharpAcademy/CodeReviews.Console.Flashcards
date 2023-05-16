using static System.Console;
using static Flashcards.CoreyJordan.ConsoleDisplay;

namespace Flashcards.CoreyJordan;
internal class FlashcardDisplay
{
    public int DisplayWidth { get; set; }

    public FlashcardDisplay(int width)
    {
        DisplayWidth = width;
    }

    public void WelcomeScreen()
    {
        WriteLine(Bar(DisplayWidth));
        WriteCenter("Welcome to Flash Card Study", DisplayWidth, 1);
        WriteCenter("Written by Corey Jordan", DisplayWidth, 2);
        WriteCenter("Developed for the C# Academy", DisplayWidth, 2);
        WriteLine(Bar(DisplayWidth));
    }

    public void MainMenu()
    {
        WriteCenter(Bar(DisplayWidth), DisplayWidth);
        WriteCenter("MAIN MENU", DisplayWidth, 1);
        WriteCenter(Bar(DisplayWidth), DisplayWidth);
        WriteLine($"{Tab(2, DisplayWidth)}N: New Study Session");
        WriteLine($"{Tab(2, DisplayWidth)}D: Decks Menu");
        WriteLine($"{Tab(2, DisplayWidth)}F: Flashcards Menu");
        WriteLine($"{Tab(2, DisplayWidth)}R: Study Report");
        WriteLine($"{Tab(2, DisplayWidth)}Q: Quit");
        Write($"\n{Tab(2, DisplayWidth)}Select an option: ");
    }

    internal void PromptUser(string message)
    {
        WriteLine($"\n{Tab(2, DisplayWidth)}{message}");
        Write(Tab(2, DisplayWidth));
        ConsoleDisplay.PromptUser();
    }
}
