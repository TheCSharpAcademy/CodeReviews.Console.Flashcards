using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.Dreamfxx.Models;

public class Flashcard
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(400)]
    public string Question { get; set; } = string.Empty;

    [Required]
    [MaxLength(450)]
    public string Answer { get; set; } = string.Empty;

    [ForeignKey(nameof(Stack))]
    public int StackId { get; set; }

    public virtual Stack? Stack { get; set; }
}

