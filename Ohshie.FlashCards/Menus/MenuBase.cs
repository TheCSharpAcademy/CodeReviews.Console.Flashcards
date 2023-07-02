namespace Ohshie.FlashCards.Menus;

public abstract class MenuBase
{
    protected abstract string[] MenuItems { get; }

    public void Initialize()
    {
        bool chosenExit = false;
        while (!chosenExit)
        {
            chosenExit = Menu();
        }
    }

    protected abstract bool Menu();

    protected string MenuBuilder(int menuSize)
    {
        return AnsiConsole.Prompt
            (
                new SelectionPrompt<string>()
                    .Title("Choose menu item:")
                    .PageSize(menuSize)
                    .AddChoices(MenuItems)
            );
    }
}