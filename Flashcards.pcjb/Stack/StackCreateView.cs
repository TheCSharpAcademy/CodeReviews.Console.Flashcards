namespace Flashcards;

class StackCreateView : BaseView
{
    private readonly StackController controller;

    public StackCreateView(StackController controller)
    {
        this.controller = controller;
    }

    public override void Body()
    {
        Console.WriteLine("New Stack");
        Console.WriteLine("Leave name empty to cancel and return to menu.");
        Console.Write("Stack-Name: ");
        var name = Console.ReadLine();
        controller.Create(name);
    }
}