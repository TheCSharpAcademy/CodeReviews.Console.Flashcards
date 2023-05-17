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

    public void ManageCards()
    {
        throw new NotImplementedException();
    }
}
