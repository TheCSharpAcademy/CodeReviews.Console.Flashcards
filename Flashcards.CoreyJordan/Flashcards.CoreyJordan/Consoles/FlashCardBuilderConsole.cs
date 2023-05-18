using ConsoleTableExt;
using Flashcards.CoreyJordan.UI;
using FlashcardsLibrary.DTOs;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Consoles;
internal class FlashCardBuilderConsole : FlashcardDisplay
{
    public int ConsoleWidth { get; set; }
    public  List<FlashCardModel> FlashCards { get; set; }

    public FlashCardBuilderConsole(int consoleWidth) : base(consoleWidth)
    {
        ConsoleWidth = consoleWidth;
        FlashCards = new List<FlashCardModel>();
    }

    internal void ManageCards()
    {
        throw new NotImplementedException();
    }

    internal void ManageCards(DeckModel deck)
    {
        bool returnToPrevious = false;
        while (returnToPrevious == false)
        {
            TitleBar($"EDIT DECK: {deck.Name.ToUpper()}");

            DeckDataObject deckDto = new(deck.Name, deck.Deck);
            ConsoleTableBuilder
                .From(deckDto.Deck)
                .WithColumn("", "Front", "Back")
                .ExportAndWriteLine();

            DeckEditMenu();
            switch (UserInput.GetString("\tSelect an option: ").ToUpper())
            {
                case "N":
                    CreateCard();
                    break;
                case "E":
                    EditCard();
                    break;
                case "D":
                    DeleteCard();
                    break;
                case "H":
                    CardHelp();
                    break;
                case "X":
                    returnToPrevious = true;
                    break;
                default:
                    PromptUser("Invalid selection. Try Again: ");
                    break;
            }
        }
    }

    private void CreateCard()
    {
        throw new NotImplementedException();
    }

    private void EditCard()
    {
        throw new NotImplementedException();
    }

    private void DeleteCard()
    {
        throw new NotImplementedException();
    }
}
