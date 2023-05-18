using ConsoleTableExt;
using Flashcards.CoreyJordan.UI;
using FlashcardsLibrary;
using FlashcardsLibrary.DTOs;
using FlashcardsLibrary.Models;
using System.Data.SqlClient;

namespace Flashcards.CoreyJordan.Consoles;
internal class DeckBuilderConsole : FlashcardDisplay
{
    public int ConsoleWidth { get; set; }
    public ConsoleDisplay ConDisplay { get; set; }
    public List<DeckModel> Decks { get; set; }
    public  List<FlashCardModel> Cards{ get; set; }

    public DeckBuilderConsole(int consoleWidth)
    {
        ConsoleWidth = consoleWidth;
        Decks = new List<DeckModel>();
        Cards = new List<FlashCardModel>();
        ConDisplay = new(consoleWidth);
    }

    public void ManageDecks()
    {
        bool returnToMain = false;
        while (returnToMain == false)
        {
            ConDisplay.TitleBar("DECK BUILDER");
            DeckBuilderMenu();
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
                        DeckBuilderHelp();
                        break;
                    case "X":
                        returnToMain = true;
                        break;
                    default:
                        PromptUser("Invalid choice, try again");
                        break;
                }
            }
            catch (SqlException ex)
            {
                PromptUser(ex.Message);
            }
        }
    }

    private void RenameDeck()
    {
        int deckChoice = SelectDeck("RENAME DECK");
        string deckName = NameDeck("RENAME DECK");

        CrudController.UpdateDeckName(deckName, deckChoice);
        PromptUser($"{deckName} renamed successfully");
    }

    

    private void DeleteDeck()
    {
        // Get a list of decks
        // Display the list
        // Get user choice
        // Confirm delete
        // Delete deck
        throw new NotImplementedException();
    }

    private void EditDeck()
    {
        int deckChoice = SelectDeck("EDIT DECK");
        DeckModel deck = CrudController.GetDeck(deckChoice);
        EditDeck(deck);
        
    }

    private void EditDeck(DeckModel deck)
    {
        FlashCardBuilderConsole editDeck = new(ConsoleWidth);
        editDeck.ManageCards(deck);

        

        // ADD NEW
        // Execute FlashCardBuilder

        // Remove
        // Get user choice
        // Remove from deck
        // Prompt remove another?
        Console.ReadLine();
    }

    private void CreateDeck()
    {
        ConDisplay.TitleBar("DECK NAME");
        string deckName = NameDeck("DECK NAME");
        
        int success = CrudController.InsertDeck(deckName);
        PromptUser($"{deckName} created successfully");

        if (success != 0)
        {
            DeckModel deck = CrudController.GetDeck(deckName);
            EditDeck(deck);
        }
    }

    private string NameDeck(string titleBar)
    {
        ConDisplay.TitleBar(titleBar);

        string deckName = UserInput.GetString($"{ConDisplay.Tab(1)}Enter a name for this deck: ");
        while (CrudController.DeckExists(deckName) == true)
        {
            PromptUser("Deck name exists. Please try again.");
            Console.Clear();
            ConDisplay.TitleBar(titleBar);
            deckName = UserInput.GetString($"{ConDisplay.Tab(1)}Enter a name for this deck: ");
        }

        return deckName;
    }
    
    private int SelectDeck(string titleBar)
    {
        ConDisplay.TitleBar(titleBar);

        List<DeckModel> decks = CrudController.GetAllDecks();
        DisplayDecks(decks);

        int deckChoice = UserInput.GetInt($"{ConDisplay.Tab(1)}Select a deck: ");
        while (decks.Any(x => x.Id == deckChoice) == false)
        {
            PromptUser("Invalid selection. Try again.");
            ConDisplay.TitleBar(titleBar);
            DisplayDecks(decks);
            deckChoice = UserInput.GetInt($"{ConDisplay.Tab(1)}Select a deck: ");
        }

        return deckChoice;
    }
}