namespace Flashcards.StevieTV.Models;

internal class FlashCard
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}

internal class FlashCardDTO
{
    public int StackId { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}

internal static class FlashCardMapper
{
    internal static FlashCardDTO FlashCardMapToDTO(FlashCard flashCard)
    {
        return new FlashCardDTO
        {
            StackId = flashCard.StackId,
            Front = flashCard.Front,
            Back = flashCard.Back
        };
    }
}