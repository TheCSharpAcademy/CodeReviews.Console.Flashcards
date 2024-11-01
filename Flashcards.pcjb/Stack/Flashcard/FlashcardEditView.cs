namespace Flashcards;

class FlashcardEditView : BaseView
{
    private readonly FlashcardController controller;
    private readonly Flashcard card;

    public FlashcardEditView(FlashcardController controller, Flashcard card)
    {
        this.controller = controller;
        this.card = card;
    }

    public override void Body()
    {
        Console.WriteLine("Edit Flashcard");
        Console.WriteLine("Leave new front or back empty to cancel and return to menu.");
        Console.WriteLine($"Old Front: {card.Front}");
        Console.WriteLine($"Old Back : {card.Back}");
        Console.Write("New Front: ");
        var front = Console.ReadLine();
        Console.Write("New Back : ");
        var back = Console.ReadLine();
        controller.Update(card.Id, front, back);
    }
}