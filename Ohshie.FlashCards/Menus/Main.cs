namespace Ohshie.FlashCards.Menus;

public class Main : MenuBase
{
    protected override string[] MenuItems { get; } =
    {
        "Solve!",
        "Show previous studies",
        "Edit decks and flashcards",
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
                GameInitMenu gameMenu = new();
                gameMenu.Initialize();
                break;
            }
            case "Show previous studies":
            {
                StudySessionsMenu sessionsMenu = new();
                sessionsMenu.Initialize();
                break;
            }
            case "Edit decks and flashcards":
            {
                ContentSettingsMenu contentSettingsMenu = new();
                contentSettingsMenu.Initialize();
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