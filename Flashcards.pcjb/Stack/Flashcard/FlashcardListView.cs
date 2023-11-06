namespace Flashcards;

using ConsoleTableExt;

class FlashcardListView : BaseView
{
    private readonly FlashcardController controller;
    private readonly Stack stack;
    private readonly List<FlashcardDto> cardDtos;
    Dictionary<long, long> publicToDatabaseId;
    private FlashcardSelectionMode mode = FlashcardSelectionMode.None;

    public FlashcardListView(FlashcardController controller, Stack stack, List<Flashcard> cards)
    {
        this.controller = controller;
        this.stack = stack;
        cardDtos = new();
        publicToDatabaseId = new();
        int publicId = 0;
        foreach (Flashcard card in cards)
        {
            publicId++;
            cardDtos.Add(new FlashcardDto(publicId, card.Front, card.Back));
            publicToDatabaseId.Add(publicId, card.Id);
        }
    }

    public void SetMode(FlashcardSelectionMode mode)
    {
        this.mode = mode;
    }

    public override void Body()
    {
        
        Console.WriteLine($"Flashcards in stack '{stack.Name}'");
        if (cardDtos != null && cardDtos.Count > 0)
        {
            ConsoleTableBuilder.From(cardDtos).ExportAndWriteLine();
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
            return;
        }
        
        if (int.TryParse(rawInput, out int publicCardId))
        {
            if (publicToDatabaseId.TryGetValue(publicCardId, out long databaseCardId))
            {
                controller.SelectCard(mode, databaseCardId);
                return;
            }
        }
        
        controller.ShowList(mode);
    }
}