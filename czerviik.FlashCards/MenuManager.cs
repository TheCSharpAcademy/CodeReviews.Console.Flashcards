namespace FlashCards;

public class MenuManager
{
    private readonly Stack<Menu> _menuStack = new Stack<Menu>();
    private readonly FlashcardDb _flashcardDb;
    private readonly StackDb _stackDb;

    public MenuManager(FlashcardDb flashcardDb, StackDb stackDb)
    {
        _flashcardDb = flashcardDb;
        _stackDb = stackDb;
        _menuStack.Push(new MainMenu(this, _flashcardDb, _stackDb));
    }

    public void DisplayCurrentMenu()
    {
        if (_menuStack.Count > 0)
        {
            Menu currentMenu = _menuStack.Peek();
            currentMenu.Display();
        }
    }

    public void NewMenu(Menu menu)
    {
        _menuStack.Push(menu);
        DisplayCurrentMenu();
    }

    public void GoBack()
    {
        if (_menuStack.Count > 1)
            _menuStack.Pop();
        DisplayCurrentMenu();
    }

    public void ReturnToMainMenu()
    {
        while (_menuStack.Count > 1)
            _menuStack.Pop();
        DisplayCurrentMenu();
    }

    public void Close()
    {
        Environment.Exit(0);
    }
}
