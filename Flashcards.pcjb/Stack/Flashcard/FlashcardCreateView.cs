namespace Flashcards;

class FlashcardCreateView : BaseView
{
    private readonly FlashcardController controller;
    private readonly Stack stack;

    public FlashcardCreateView(FlashcardController controller, Stack stack)
    {
        this.controller = controller;
        this.stack = stack;
    }

    public override void Body()
    {
        Console.WriteLine($"New Flashcard for Stack '{stack.Name}'");
        Console.WriteLine("Leave front or back empty to cancel and return to menu.");
        Console.Write("Front: ");
        var front = Console.ReadLine();
        Console.Write("Back : ");
        var back = Console.ReadLine();
        controller.Create(stack, front, back);
    }
}