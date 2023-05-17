using Flashcards.CoreyJordan.UI;
using FlashcardsLibrary;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Consoles;
internal class DeckBuilderConsole
{
    public int ConsoleWidth { get; set; }
    public ConsoleDisplay ConDisplay { get; set; }
    public FlashcardDisplay FlashDisplay { get; set; }
    public List<StackModel> Decks { get; set; }
    public  List<FlashCardModel> Cards{ get; set; }

    public DeckBuilderConsole(int consoleWidth)
    {
        ConsoleWidth = consoleWidth;
        Decks = new List<StackModel>();
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
        StackModel newDeck = new()
        {
            Name = UserInput.GetString("Enter a name for this deck")
        };

        while (DataValidation.IsUniqueDeckName(newDeck.Name) == false)
        {
            FlashDisplay.PromptUser("Deck name exists. Please try again.");
            newDeck.Name = UserInput.GetString("Enter a name for this deck");
        }
        // TODO Get list of cards
        // TODO Add cards to deck
        // TODO Save deck to database
    }
}
