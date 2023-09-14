namespace Flashcards;

class FlashcardMenuView : BaseView
{
    private readonly FlashcardController controller;
    private readonly Stack? activeStack;

    public FlashcardMenuView(FlashcardController controller, Stack? activeStack)
    {
        this.controller = controller;
        this.activeStack = activeStack;
    }

    public override void Body()
    {
        Console.WriteLine("Manage Flashcards");
        if (activeStack != null)
        {
            Console.WriteLine($"Active Stack: {activeStack.Name}");
        }
        Console.WriteLine("1 - List Flashcards");
        Console.WriteLine("2 - Add Flashcard");
        Console.WriteLine("3 - Edit Flashcard");
        Console.WriteLine("4 - Delete Flashcard");
        Console.WriteLine("5 - Change Stack");
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
            case "5":
                controller.ChangeStack();
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