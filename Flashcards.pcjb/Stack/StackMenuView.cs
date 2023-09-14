namespace Flashcards;

class StackMenuView : BaseView
{
    private readonly StackController controller;
    private readonly Stack? activeStack;

    public StackMenuView(StackController controller, Stack? activeStack)
    {
        this.controller = controller;
        this.activeStack = activeStack;
    }

    public override void Body()
    {
        Console.WriteLine("Manage Stacks");
        if (activeStack != null)
        {
            Console.WriteLine($"Active Stack: {activeStack.Name}");
        }
        Console.WriteLine("1 - List Stacks & Select Active Stack");
        Console.WriteLine("2 - Add Stack");
        Console.WriteLine("3 - Edit Stack");
        Console.WriteLine("4 - Delete Stack");
        Console.WriteLine("0 - Return to Main Menu");
        Console.WriteLine("Enter one of the numbers above to select a menu option.");

        switch (Console.ReadKey().KeyChar.ToString())
        {
            case "1":
                controller.ShowList();
                break;
            case "2":
                controller.ShowCreate();
                break;
            case "3":
                controller.ShowEdit();
                break;
            case "4":
                controller.ShowDelete();
                break;
            case "0":
                controller.BackToMainMenu();
                break;
            default:
                controller.ShowMenu();
                break;
        }
    }
}