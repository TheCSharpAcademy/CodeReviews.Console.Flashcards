namespace Ohshie.FlashCards.Menus;

public class Main : MenuBase
{
    protected override string[] MenuItems { get; } = new[]
    {
        "Solve!",
        "Show previous studies",
        "Edit flashcards",
        "Exit"
    };

    public new void Initialize()
    {
        bool chosenExit = false;
        while (!chosenExit)
        {
            chosenExit = Menu();
        }
        Environment.Exit(0);
    }
    
    protected override bool Menu()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Learning the right way."));

        var userChoise = MenuBuilder(5);

        switch (userChoise)
        {
            case "Solve!":
            {
                GameMenu gameMenu = new GameMenu();
                gameMenu.Initialize();
                break;
            }
            case "Show previous studies":
            {
                break;
            }
            case "Edit flashcards":
            {
                break;
            }
            case "Exit":
            {
                return true;
            }
        }

        return false;
    }
}