using Flashcards.CoreyJordan.UI;
using FlashcardsLibrary;
using FlashcardsLibrary.Models;
using System.Data.SqlClient;

namespace Flashcards.CoreyJordan.Consoles;
internal class DeckBuilderConsole : FlashcardDisplay
{
    public DeckBuilderConsole(int consoleWidth) : base(consoleWidth)
    {
        ConsoleWidth = consoleWidth;
        Decks = new List<DeckModel>();
        Cards = new List<FlashCardModel>();
    }

    public int ConsoleWidth { get; set; }
    public List<DeckModel> Decks { get; set; }
    public  List<FlashCardModel> Cards{ get; set; }

    

    public void ManageDecks()
    {
        bool returnToMain = false;
        while (returnToMain == false)
        {
            TitleBar("DECK BUILDER");
            DeckBuilderMenu();
            Console.WriteLine();
            try
            {
                switch (UserInput.GetString($"{Tab(1)}What would you like to do? ").ToUpper())
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
        int deckChoice = SelectDeck("EDIT DECK");
        string confirm = UserInput.GetString("\tDo you really want to delete this deck? Y/n: ");

        if ( confirm.ToUpper() == "Y")
        {
            int success = CrudController.DeleteDeck(deckChoice);
            if (success != 0)
            {
                PromptUser("Deck deleted successfully");
            }
        }
        else
        {
            PromptUser("Canceled");
        }
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
    }

    private void CreateDeck()
    {
        TitleBar("DECK NAME");
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
        TitleBar(titleBar);

        string deckName = UserInput.GetString($"{Tab(1)}Enter a name for this deck: ");
        while (CrudController.DeckExists(deckName) == true)
        {
            PromptUser("Deck name exists. Please try again.");
            Console.Clear();
            TitleBar(titleBar);
            deckName = UserInput.GetString($"{Tab(1)}Enter a name for this deck: ");
        }

        return deckName;
    }
    
    private int SelectDeck(string titleBar)
    {
        TitleBar(titleBar);

        List<DeckModel> decks = CrudController.GetAllDecks();
        DisplayDecks(decks);

        int deckChoice = UserInput.GetInt($"{Tab(1)}Select a deck: ");
        while (decks.Any(x => x.Id == deckChoice) == false)
        {
            PromptUser("Invalid selection. Try again.");
            TitleBar(titleBar);
            DisplayDecks(decks);
            deckChoice = UserInput.GetInt($"{Tab(1)}Select a deck: ");
        }

        return deckChoice;
    }
}