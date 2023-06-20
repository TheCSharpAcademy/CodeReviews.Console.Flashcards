namespace Ohshie.FlashCards.DataAccess;

public class FlashCardDto
{
    public int DtoId { get; set; }
    public int FlashCardId { get; set; }
    public int DeckId { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
}