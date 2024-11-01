using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.TwilightSaw.Domain;

public class Flashcard
{
    [Key]
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }

    public int StackId { get; set; }

    public CardStack CardStack { get; set; }
}