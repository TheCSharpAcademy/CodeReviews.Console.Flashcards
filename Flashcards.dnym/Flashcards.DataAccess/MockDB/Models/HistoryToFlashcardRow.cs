namespace Flashcards.DataAccess.MockDB.Models;

internal class HistoryToFlashcardRow
{
    private static int _idCounter = 1;

    public HistoryToFlashcardRow(int historyIdFK, int flashcardIdFK, bool wasCorrect, DateTime answeredAt)
    {
        IdPK = _idCounter++;
        HistoryIdFK = historyIdFK;
        FlashcardIdFK = flashcardIdFK;
        WasCorrect = wasCorrect;
        AnsweredAt = answeredAt;
    }

    public int IdPK { get; }
    public int HistoryIdFK { get; }
    public int FlashcardIdFK { get; }
    public bool WasCorrect { get; }
    public DateTime AnsweredAt { get; }
}
