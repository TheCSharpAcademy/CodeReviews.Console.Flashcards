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

    internal void FlashCard(string cardFace)
    {
        WriteLine(Tab(1, DisplayWidth) + CardConstructor("+", "-"));
        WriteLine(Tab(1, DisplayWidth) + CardConstructor("|", " "));
        WriteLine(Tab(1, DisplayWidth) + CardConstructor("|", " ", cardFace));
        WriteLine(Tab(1, DisplayWidth) + CardConstructor("|", " "));
        WriteLine(Tab(1, DisplayWidth) + CardConstructor("+", "-"));
    }

    private string CardConstructor(string border, string fill)
    {
        string cardLine = border;
        for (int i = 0; i < (DisplayWidth - 16); i++)
        {
            cardLine += fill;
        }
        cardLine += border;

        return cardLine;
    }

    private string CardConstructor(string border, string fill, string face)
    {
        int whiteSpace = DisplayWidth - 16 - (face.Length);

        string cardLine = border;
        for (int i = 0; i < whiteSpace / 2; i++)
        {
            cardLine += fill;
        }
        cardLine += face;
        for (int i = 0; i < whiteSpace / 2; i++)
        {
            cardLine += " ";
        }
        cardLine += border;

        return cardLine;
    }

}
