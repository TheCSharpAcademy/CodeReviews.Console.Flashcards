namespace Flashcards;

using ConsoleTableExt;

class FlashcardListView : BaseView
{
    private readonly FlashcardController controller;
    private readonly Stack stack;
    private readonly List<FlashcardDto> cards;
    private FlashcardSelectionMode mode = FlashcardSelectionMode.None;

    public FlashcardListView(FlashcardController controller, Stack stack, List<FlashcardDto> cards)
    {
        this.controller = controller;
        this.stack = stack;
        this.cards = cards;
    }

    public void SetMode(FlashcardSelectionMode mode)
    {
        this.mode = mode;
    }

    public override void Body()
    {
        Console.WriteLine($"Flashcards in stack '{stack.Name}'");
        if (cards != null && cards.Count > 0)
        {
            ConsoleTableBuilder.From(cards).ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("No flashcards found.");
        }

        switch (mode)
        {
            case FlashcardSelectionMode.ForEdit:
                Console.WriteLine("Enter ID and press enter to edit the card.");
                break;
            case FlashcardSelectionMode.ForDelete:
                Console.WriteLine("Enter ID and press enter to delete the card.");
                break;
        }
        
        Console.WriteLine("Press enter alone to return to main menu.");

        var rawInput = Console.ReadLine();
        if (String.IsNullOrEmpty(rawInput))
        {
            controller.ShowMenu();
        }
        else if (int.TryParse(rawInput, out int cardId))
        {
            controller.SelectCard(mode, cardId);
        }
        else
        {
            controller.ShowList(mode);
        }
    }
}