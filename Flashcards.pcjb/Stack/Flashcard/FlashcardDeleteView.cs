namespace Flashcards;

class FlashcardDeleteView : BaseView
{
    private readonly FlashcardController controller;
    private readonly Flashcard card;

    public FlashcardDeleteView(FlashcardController controller, Flashcard card)
    {
        this.controller = controller;
        this.card = card;
    }

    public override void Body()
    {
        Console.WriteLine("Delete Flashcard");
        Console.WriteLine($"Front: {card.Front}");
        Console.WriteLine($"Back : {card.Back}");
        Console.WriteLine("Are you sure? [y/n]");
        switch (Console.ReadKey().KeyChar.ToString())
        {
            case "y":
                controller.Delete(card);
                break;
            default:
                controller.ShowMenu();
                break;
        }
    }
}