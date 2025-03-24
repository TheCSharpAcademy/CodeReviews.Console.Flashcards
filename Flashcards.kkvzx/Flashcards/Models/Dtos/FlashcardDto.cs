namespace Flashcards.Models.Dtos;

public class FlashcardDto : BaseEntity
{
    public int StackId { get; init; }
    public string FrontText { get; init; }
    public string BackText { get; init; }

    public FlashcardDto()
    {
    }

    public FlashcardDto(int stackId, string frontText, string backText)
    {
        StackId = stackId;
        FrontText = frontText;
        BackText = backText;
    }
    
    public FlashcardDto(int id, int stackId, string frontText, string backText)
    {
        Id = id;
        StackId = stackId;
        FrontText = frontText;
        BackText = backText;
    }
}