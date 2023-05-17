using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Consoles;
internal class StudySessionConsole
{
    public int ConsoleWidth { get; set; }
    public string UserName { get; set; }
    public DeckModel Deck { get; set; }
    public int CorrectAnswers { get; set; }
    public List<FlashCardModel> Correct { get; set; } = new();
    public List<FlashCardModel> Wrong { get; set; } = new();

    public StudySessionConsole(int consoleWidth)
    {
        ConsoleWidth = consoleWidth;

    }

    internal void Study()
    {
        // TODO Check if decks exist
        throw new NotImplementedException();
    }
}
