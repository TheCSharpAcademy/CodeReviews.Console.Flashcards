namespace Flashcards.DataAccess.MockDB.Models;

internal class HistoryRow
{
    private static int _idCounter = 1;

    public HistoryRow(int StackId, DateTime startedAt)
    {
        IdPK = _idCounter++;
        StackIdFK = StackId;
        StartedAt = startedAt;
    }

    public int IdPK { get; }
    public int StackIdFK { get; }
    public DateTime StartedAt { get; }
}
