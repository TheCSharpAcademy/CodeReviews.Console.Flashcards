using Flashcards.CoreyJordan.UI;
using static System.Console;

namespace Flashcards.CoreyJordan;
internal class FlashcardDisplay
{
    public int DisplayWidth { get; set; }
    public ConsoleDisplay Display { get; set; }

    internal FlashcardDisplay(int width)
    {
        DisplayWidth = width;
        Display = new(DisplayWidth);
    }

    internal void WelcomeScreen()
    {
        WriteLine(Display.Bar());
        Display.WriteCenter("Welcome to Flash Card Study", 1);
        Display.WriteCenter("Written by Corey Jordan", 2);
        Display.WriteCenter("Developed for the C# Academy", 2);
        WriteLine(Display.Bar());
    }

    internal void MainMenu()
    {
        Clear();
        Display.TitleBar("MAIN MENU");
        WriteLine($"{Display.Tab(2)}N: New Study Session");
        WriteLine($"{Display.Tab(2)}D: Decks Menu");
        WriteLine($"{Display.Tab(2)}F: Flashcards Menu");
        WriteLine($"{Display.Tab(2)}R: Study Report");
        WriteLine($"{Display.Tab(2)}Q: Quit");
        Write($"\n{Display.Tab(2)}Select an option: ");
    }

    public void DeckBuilderMenu()
    {
        WriteLine($"{Display.Tab(1)}N: New Study Deck");
        WriteLine($"{Display.Tab(1)}E: Edit Existing Deck");
        WriteLine($"{Display.Tab(1)}D: Delete Existing Deck");
        WriteLine($"{Display.Tab(1)}X: Return to Main Menu");
    }

    internal void PromptUser(string message)
    {
        WriteLine($"\n{Display.Tab(2)}{message}");
        Write(Display.Tab(2));
        Display.PromptUser();
    }

    internal void FlashCard(string cardFace)
    {
        WriteLine(Display.Tab(1) + CardConstructor("+", "-"));
        WriteLine(Display.Tab(1) + CardConstructor("|", " "));
        WriteLine(Display.Tab(1) + CardConstructor("|", " ", cardFace));
        WriteLine(Display.Tab(1) + CardConstructor("|", " "));
        WriteLine(Display.Tab(1) + CardConstructor("+", "-"));
    }

    private string CardConstructor(string border, string fill)
    {
        string cardLine = border;
        for (int i = 0; i < DisplayWidth - 16; i++)
        {
            cardLine += fill;
        }
        cardLine += border;

        return cardLine;
    }

    private string CardConstructor(string border, string fill, string face)
    {
        int whiteSpace = DisplayWidth - 16 - face.Length;
        int left = whiteSpace / 2;
        int right = whiteSpace - left;

        string cardLine = border;
        for (int i = 0; i < left; i++)
        {
            cardLine += fill;
        }
        cardLine += face;
        for (int i = 0; i < right; i++)
        {
            cardLine += " ";
        }
        cardLine += border;

        return cardLine;
    }

}
