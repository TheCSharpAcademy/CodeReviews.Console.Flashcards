using System.ComponentModel.DataAnnotations;

namespace Flashcards.TwilightSaw.Models;

public class Flashcard
{
    [Key]
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }

    public int CardStackId { get; set; }

    public CardStack CardStack { get; set; }

    public Flashcard(string front, string back, int cardStackId )
    {
        Front = front;
        Back = back;
        CardStackId = cardStackId;
    }

    public override string ToString()
    {
        return $@"{Front} {Back}";
    }
}