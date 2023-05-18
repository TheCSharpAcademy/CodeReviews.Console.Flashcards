using Flashcards.CoreyJordan.UI;
using FlashcardsLibrary;
using FlashcardsLibrary.Models;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Flashcards.CoreyJordan.Consoles;
internal class DeckBuilderConsole
{
    public int ConsoleWidth { get; set; }
    public ConsoleDisplay ConDisplay { get; set; }
    public FlashcardDisplay FlashDisplay { get; set; }
    public List<DeckModel> Decks { get; set; }
    public  List<FlashCardModel> Cards{ get; set; }

    public DeckBuilderConsole(int consoleWidth)
    {
        ConsoleWidth = consoleWidth;
        Decks = new List<DeckModel>();
        Cards = new List<FlashCardModel>();
        ConDisplay = new(consoleWidth);
        FlashDisplay = new(consoleWidth);
    }

    public void ManageDecks()
    {
        bool returnToMain = false;
        while (returnToMain == false)
        {
            ConDisplay.TitleBar("DECK BUILDER");
            FlashDisplay.DeckBuilderMenu();
            Console.WriteLine();
            try
            {
                switch (UserInput.GetString($"{ConDisplay.Tab(1)}What would you like to do? ").ToUpper())
                {
                    case "N":
                        CreateDeck();
                        break;
                    case "E":
                        EditDeck();
                        break;
                    case "R":
                        RenameDeck();
                        break;
                    case "D":
                        DeleteDeck();
                        break;
                    case "H":
                        FlashDisplay.DeckBuilderHelp();
                        break;
                    case "X":
                        returnToMain = true;
                        break;
                    default:
                        FlashDisplay.PromptUser("Invalid choice, try again");
                        break;
                }
            }
            catch (SqlException ex)
            {
                FlashDisplay.PromptUser(ex.Message);
            }
        }
    }

    private void RenameDeck()
    {
        ConDisplay.TitleBar("RENAME DECK");

        List<DeckModel> decks = CrudController.GetAllDecks();
        FlashDisplay.DisplayDecks(decks);

        int deckChoice = UserInput.GetInt($"{ConDisplay.Tab(1)}Select a deck: ");
        while (decks.Any(x => x.Id == deckChoice) == false)
        {
            FlashDisplay.PromptUser("Invalid selection. Try again.");
            ConDisplay.TitleBar("RENAME DECK");
            FlashDisplay.DisplayDecks(decks);
            deckChoice = UserInput.GetInt($"{ConDisplay.Tab(1)}Select a deck: ");
        }

        ConDisplay.TitleBar("RENAME DECK");
        string deckName = NameDeck("RENAME DECK");

        CrudController.UpdateDeckName(deckName, deckChoice);
        FlashDisplay.PromptUser("Deck updated successfully");
    }

    private void DeleteDeck()
    {
        throw new NotImplementedException();
    }

    private void EditDeck()
    {
        throw new NotImplementedException();
    }

    private void CreateDeck()
    {
        ConDisplay.TitleBar("DECK NAME");
        string deckName = NameDeck("DECK NAME");
        
        CrudController.InsertDeck(deckName);
        FlashDisplay.PromptUser("Deck created successfully");
    }

    private string NameDeck(string titleBar)
    {
        string deckName = UserInput.GetString($"{ConDisplay.Tab(1)}Enter a name for this deck: ");
        while (CrudController.DeckExists(deckName) == true)
        {
            FlashDisplay.PromptUser("Deck name exists. Please try again.");
            Console.Clear();
            ConDisplay.TitleBar(titleBar);
            deckName = UserInput.GetString($"{ConDisplay.Tab(1)}Enter a name for this deck: ");
        }
        return deckName;
    }
}
