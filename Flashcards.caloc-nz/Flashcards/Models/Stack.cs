using System.ComponentModel.DataAnnotations;

namespace Flashcards.Models;

public class Stack
{
    public int Id { get; set; }

    [Required] [MaxLength(250)] public string Name { get; set; } = string.Empty;

    public virtual List<Flashcard> Flashcards { get; set; } = new();
}