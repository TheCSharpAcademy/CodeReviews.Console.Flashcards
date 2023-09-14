namespace Flashcards;

using ConsoleTableExt;

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
        Console.Write("Stack-Name: ");
        var name = Console.ReadLine();
        controller.Create(name);
    }
}