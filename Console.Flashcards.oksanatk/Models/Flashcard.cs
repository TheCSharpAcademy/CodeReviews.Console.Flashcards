using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetMAUI.Flashcards.Models;
public class Flashcard
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(StackId))]
    public int StackId { get; set; }
    public int DisplayNum { get; set; }
    public Stack Stack { get; set; } = null!;
    public string Front { get; set; } = null!;
    public string Back { get; set; } = null!;
}
