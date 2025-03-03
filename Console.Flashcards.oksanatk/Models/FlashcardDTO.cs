using System.ComponentModel.DataAnnotations;

namespace dotnetMAUI.Flashcards.Models;

public class FlashcardDto
{
    [Key]
    public int Id { get; set; }
    public Stack Stack { get; set; } = null!;
    public int DisplayNum { get; set; }
    public string Front { get; set; } = null!;
    public string Back { get; set; } = null!;
}
