namespace Flashcards;

class StackMenuView : BaseView
{
    private readonly StackController controller;

    public StackMenuView(StackController controller)
    {
        this.controller = controller;
    }

    public override void Body()
    {
        Console.WriteLine("Manage Stacks");
        Console.WriteLine("1 - List Stacks");
        Console.WriteLine("2 - Add Stack");
        Console.WriteLine("3 - Edit Stack");
        Console.WriteLine("4 - Delete Stack");
        Console.WriteLine("0 - Return to Main Menu");
        Console.WriteLine("Enter one of the numbers above to select a menu option.");

        switch (Console.ReadKey().KeyChar.ToString())
        {
            case "1":
                controller.ShowList(StackSelectionMode.None);
                break;
            case "2":
                controller.ShowCreate();
                break;
            case "3":
                controller.ShowList(StackSelectionMode.ForEdit);
                break;
            case "4":
                controller.ShowList(StackSelectionMode.ForDelete);
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