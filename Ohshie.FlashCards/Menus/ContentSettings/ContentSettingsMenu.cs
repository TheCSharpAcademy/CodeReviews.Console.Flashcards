using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.Menus;

public class ContentSettingsMenu : MenuBase
{
    protected override string[] MenuItems { get; } =
    {
        "Create new Deck",
        "Edit current Deck",
        "Go back"
    };
    
    protected override bool Menu()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Settings"));

        string userChoice = MenuBuilder(3);
        
        switch (userChoice)
        {
            case "Create new Deck":
            {
                DeckCreator deckCreator = new();
                deckCreator.Create();
                break;
            }
            case "Edit current Deck":
            {
                EditDecksMenu editDecksMenu = new();
                break;
            }
            case "Go back":
            {
                return true;
            }
        }

        return false;
    }
}