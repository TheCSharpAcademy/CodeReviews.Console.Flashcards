namespace Flashcards.DataAccess.MockDB.Models;

internal class FlashcardsRow
{
    private static int _idCounter = 1;

    public FlashcardsRow(int stackId, string front, string back)
    {
        IdPK = _idCounter++;
        StackIdFK = stackId;
        Front = front;
        Back = back;
    }

    public int IdPK { get; }
    public int StackIdFK { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
}
