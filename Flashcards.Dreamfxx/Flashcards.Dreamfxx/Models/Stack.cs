using System.ComponentModel.DataAnnotations;

namespace Flashcards.Dreamfxx.Models;

public class Stack
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(400)]
    public string Description { get; set; } = string.Empty;
    public virtual List<Flashcard> Flashcards { get; set; } = new();
    public virtual List<Session> Sessions { get; set; } = new();
}
