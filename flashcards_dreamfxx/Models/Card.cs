using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.DreamFXX.Models;

public class Card
{
    public int Id { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }

    [ForeignKey("CardStackId")]
    public int CardStackId { get; set; }
    public StackOfCards? StackOfCardsList { get; set; }

}
