using ConsoleTableExt;
using Flashcards.CoreyJordan.UI;
using FlashcardsLibrary.DTOs;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Consoles;
internal class FlashCardBuilderConsole
{
    public int ConsoleWidth { get; set; }
    public  List<FlashCardModel> FlashCards { get; set; }

    public FlashCardBuilderConsole(int consoleWidth)
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
        ConsoleDisplay conDisplay = new(ConsoleWidth);
        FlashcardDisplay display = new(ConsoleWidth);

        bool returnToEdit = false;
        while (returnToEdit == false)
        {
            ConDisplay.TitleBar($"EDIT DECK: {deck.Name.ToUpper()}");

            DeckDataObject deckDto = new(deck.Name, deck.Deck);
            ConsoleTableBuilder
                .From(deckDto.Deck)
                .WithColumn("", "Front", "Back")
                .ExportAndWriteLine();

            FlashDisplay.DeckEditMenu();
            switch (UserInput.GetString("\tSelect an option: "))
            {
                case "N":
                    FlashCardBuilderConsole newCard = new(ConsoleWidth);
                    newCard.DeleteCard();
                    break;
                case "R":
                    FlashCardBuilderConsole deleteCard = new(ConsoleWidth);
                    deleteCard.DeleteCard();
                    break;
                case "X":
                    returnToEdit = true;
                    break;
                default:
                    FlashDisplay.PromptUser("Invalid selection. Try Again: ");
                    break;
            }
        }
    }

    internal void DeleteCard()
    {
        throw new NotImplementedException();
    }
}
