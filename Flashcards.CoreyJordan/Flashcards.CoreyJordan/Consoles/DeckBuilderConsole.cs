using Flashcards.CoreyJordan.UI;
using FlashcardsLibrary;
using FlashcardsLibrary.Models;
using System.Data.SqlClient;

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
            Console.Clear();
            ConDisplay.TitleBar("Deck Builder");
            FlashDisplay.DeckBuilderMenu();
            Console.WriteLine();

            switch (UserInput.GetString($"{ConDisplay.Tab(1)}What would you like to do? ").ToUpper())
            {
                case "N":
                    CreateDeck();
                    EditDeck();
                    break;
                case "E":
                    EditDeck();
                    break;
                case "D":
                    DeleteDeck();
                    break;
                case "X":
                    returnToMain = true;
                    break;
                default:
                    FlashDisplay.PromptUser("Invalid choice, try again");
                    break;
            }
        }
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
        string deckName = UserInput.GetString("Enter a name for this deck");
        try
        {
            while (CrudController.DeckExists(deckName) == true)
            {
                FlashDisplay.PromptUser("Deck name exists. Please try again.");
                deckName = UserInput.GetString("Enter a name for this deck");
            }
        
            CrudController.InsertDeck(deckName);
            ConDisplay.WriteCenter("Deck created successfully");
        }
        catch (SqlException ex)
        {
            FlashDisplay.PromptUser(ex.Message);
        }
    }
}
