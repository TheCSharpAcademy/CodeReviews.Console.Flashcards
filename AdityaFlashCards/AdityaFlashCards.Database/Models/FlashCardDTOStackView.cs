namespace AdityaFlashCards.Database.Models;

public class FlashCardDtoStackView
{
    public string? Question { get; set; }
    public string? Answer { get; set; }
    public int PositionInStack { get; set; }
}