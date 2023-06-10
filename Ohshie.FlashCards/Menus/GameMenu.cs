namespace Ohshie.FlashCards.Menus;

public class GameMenu : MenuBase
{
    protected override string[] MenuItems { get; } =
    {
        "Placeholder"
    };

    public new void Initialize()
    {
        if(!Verify.StackExist()) return;
        
        bool chosenExit = false;
        while (!chosenExit)
        {
            chosenExit = Menu();
        }
    }
    
    protected override bool Menu()
    {
        throw new NotImplementedException();
    }
}