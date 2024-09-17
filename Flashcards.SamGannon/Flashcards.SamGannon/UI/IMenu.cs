using DataAccess;

namespace Flashcards.SamGannon.UI;

public interface IMenu
{
    IDataAccess DataAccess { get; }
    void ShowMenu();
}
