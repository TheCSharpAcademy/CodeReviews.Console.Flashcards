using static System.Console;
using static Flashcards.CoreyJordan.ConsoleDisplay;

namespace Flashcards.CoreyJordan;
internal class FlashcardDisplay
{
    private int DisplayWidth { get; set; }

    internal FlashcardDisplay(int width)
    {
        DisplayWidth = width;
    }

    internal void WelcomeScreen()
    {
        WriteLine(Bar(DisplayWidth));
        WriteCenter("Welcome to Flash Card Study", DisplayWidth, 1);
        WriteCenter("Written by Corey Jordan", DisplayWidth, 2);
        WriteCenter("Developed for the C# Academy", DisplayWidth, 2);
        WriteLine(Bar(DisplayWidth));
    }

    internal void MainMenu()
    {
        Clear();
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
