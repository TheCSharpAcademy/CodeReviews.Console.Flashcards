using System.ComponentModel.DataAnnotations;

namespace Flashcards.kalsson.Models;

public class Flashcard
{
    [Key]
    public int FlashcardId { get; set; }
    public int StackId { get; set; }
    public string FrontText { get; set; }
    public string BackText { get; set; }
    public virtual Stack Stack { get; set; }
}