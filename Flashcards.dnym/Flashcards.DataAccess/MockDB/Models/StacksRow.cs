namespace Flashcards.DataAccess.MockDB.Models;

internal class StacksRow
{
    private static int _idCounter = 1;

    public StacksRow(string sortName, string viewName)
    {
        IdPK = _idCounter++;
        SortNameUQ = sortName;
        ViewName = viewName;
    }

    public int IdPK { get; }
    public string SortNameUQ { get; set; }
    public string ViewName { get; set; }
}
