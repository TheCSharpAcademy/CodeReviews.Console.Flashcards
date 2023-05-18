using Flashcards.CoreyJordan.UI;
using FlashcardsLibrary.Models;
using static System.Console;

namespace Flashcards.CoreyJordan;
internal class FlashcardDisplay : ConsoleDisplay
{
    public FlashcardDisplay(int consoleWidth) : base(consoleWidth)
    {
    }

    public int DisplayWidth { get; set; }

    internal void WelcomeScreen()
    {
        WriteLine(Bar());
        WriteCenter("Welcome to Flash Card Study", 1);
        WriteCenter("Written by Corey Jordan", 2);
        WriteCenter("Developed for the C# Academy", 2);
        WriteLine(Bar());
    }

    internal void MainMenu()
    {
        Clear();
        TitleBar("MAIN MENU");
        WriteLine($"{Tab(2)}N: New Study Session");
        WriteLine($"{Tab(2)}D: Decks Menu");
        WriteLine($"{Tab(2)}F: Flashcards Menu");
        WriteLine($"{Tab(2)}R: Study Report");
        WriteLine($"{Tab(2)}Q: Quit");
        Write($"\n{Tab(2)}Select an option: ");
    }

    public void DeckBuilderMenu()
    {
        WriteLine($"{Tab(1)}N: New Study Deck");
        WriteLine($"{Tab(1)}E: Edit Existing Deck");
        WriteLine($"{Tab(1)}R: Rename Existing Deck");
        WriteLine($"{Tab(1)}D: Delete Existing Deck");
        WriteLine($"{Tab(1)}H: Help");
        WriteLine($"{Tab(1)}X: Return to Main Menu");
    }

    internal void PromptUser(string message)
    {
        WriteLine();
        WriteCenter(message, 1);
        PromptUser();
    }

    internal void FlashCard(string cardFace)
    {
        WriteLine(Tab(1) + CardConstructor("+", "-"));
        WriteLine(Tab(1) + CardConstructor("|", " "));
        WriteLine(Tab(1) + CardConstructor("|", " ", cardFace));
        WriteLine(Tab(1) + CardConstructor("|", " "));
        WriteLine(Tab(1) + CardConstructor("+", "-"));
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

    public void DisplayDecks(List<DeckModel> decks)
    {
        foreach (DeckModel deck in decks)
        {
            WriteLine($"{Tab(1)}{deck.Id}: {deck.Name}");
        }
    }

    internal void DeckBuilderHelp()
    {
        TitleBar("HELP");

        WriteLine("New Study Deck\n");
        WriteLine("\tCreate a new deck and give it a unique name. The");
        WriteLine("\t\tname cannot be left blank.\n");

        WriteLine("Edit Existing Deck\n");
        WriteLine("\tAdd or remove flashcards from deck lists. Each card");
        WriteLine("\t\tcan belong to only one deck at a time.\n");
            
        WriteLine("Rename Existing Deck\n");
        WriteLine("\tSelect a deck and enter a new unique name.\n");

        WriteLine("Delete Existing Deck\n");
        WriteLine("\tSelect a deck and remove it from the program.\n");
        WriteLine("\t *** THIS WILL DELETE ALL DECK CONTENTS ***");
        WriteLine("\t *** THIS ACTION CANNOT BE UNDONE ***\n");

        PromptUser();
    }

    internal void CardHelp()
    {

    }

    internal void DeckEditMenu()
    {
        WriteLine($"{Tab(1)}N: Create New Card");
        WriteLine($"{Tab(1)}E: Edit Existing Card");
        WriteLine($"{Tab(1)}D: Delete Card");
        WriteLine($"{Tab(1)}H: Help");
        WriteLine($"{Tab(1)}X: Return to Main Menu");
    }
}
