using FlashcardsLibrary.Models;

public class FlashCardDataObject
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }

    public FlashCardDataObject(FlashCardModel card)
    {
        Front = card.Front;
        Back = card.Back;
    }
}
