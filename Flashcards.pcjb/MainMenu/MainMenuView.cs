namespace Flashcards;

class MainMenuView : BaseView
{
    private MainMenuController controller;

    public MainMenuView(MainMenuController controller)
    {
        this.controller = controller;
    }

    public override void Body()
    {
        Console.WriteLine("1 - Manage Stacks");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("Enter one of the numbers above to select a menu option.");

        switch (Console.ReadKey().KeyChar.ToString())
        {
            case "1":
                controller.ManageStacks();
                break;
            case "0":
                controller.Exit();
                break;
            default:
                controller.ShowMainMenu();
                break;
        }
    }
}