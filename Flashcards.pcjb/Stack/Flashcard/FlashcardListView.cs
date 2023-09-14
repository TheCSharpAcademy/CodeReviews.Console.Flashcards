namespace Flashcards;

using ConsoleTableExt;

class FlashcardListView : BaseView
{
    private readonly FlashcardController controller;
    private readonly Stack stack;
    private readonly List<FlashcardDto> cards;

    public FlashcardListView(FlashcardController controller, Stack stack, List<FlashcardDto> cards)
    {
        this.controller = controller;
        this.stack = stack;
        this.cards = cards;
    }

    public override void Body()
    {
        Console. WriteLine($"Flashcards in stack '{stack.Name}'");
        if (cards != null && cards.Count > 0)
        {
            ConsoleTableBuilder.From(cards).ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("No flashcards found.");
        }

        Console.WriteLine("Press enter alone to return to main menu.");
        var rawInput = Console.ReadLine();
        if (String.IsNullOrEmpty(rawInput))
        {
            controller.ShowMenu();
        }
        else
        {
            controller.ShowList();
        }
    }
}