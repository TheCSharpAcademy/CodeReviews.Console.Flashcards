namespace Flashcards;

using ConsoleTableExt;

class StackDeleteView : BaseView
{
    private readonly StackController controller;
    private readonly Stack stack;

    public StackDeleteView(StackController controller, Stack stack)
    {
        this.controller = controller;
        this.stack = stack;
    }

    public override void Body()
    {
        Console.WriteLine("Delete Stack");
        Console.WriteLine($"Stack-Name: {stack.Name}");
        Console.WriteLine("All flashcards and study sessions belonging to this stack will be deleted!");
        Console.WriteLine("Are you sure? [y/n]");
        switch (Console.ReadKey().KeyChar.ToString())
        {
            case "y":
                controller.Delete(stack);
                break;
            default:
                controller.ShowMenu();
                break;
        }
    }
}