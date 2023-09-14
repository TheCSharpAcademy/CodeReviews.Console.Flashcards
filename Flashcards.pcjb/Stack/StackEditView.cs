namespace Flashcards;

class StackEditView : BaseView
{
    private readonly StackController controller;
    private readonly Stack stack;

    public StackEditView(StackController controller, Stack stack)
    {
        this.controller = controller;
        this.stack = stack;
    }

    public override void Body()
    {
        Console.WriteLine("Edit Stack");
        Console.WriteLine("Leave new name empty to cancel and return to menu.");
        Console.WriteLine($"Old Stack-Name: {stack.Name}");
        Console.Write("New Stack-Name: ");
        var name = Console.ReadLine();
        controller.Update(stack.Id, name);
    }
}