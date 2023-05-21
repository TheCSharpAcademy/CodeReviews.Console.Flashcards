namespace Flashcards.CoreyJordan.Display;
internal class MenuModel
{
    public string Index { get; set; }
    public string MenuItem { get; set; }

    public MenuModel(string index, string menuItem)
    {
        Index = index;
        MenuItem = menuItem;
    }
}
