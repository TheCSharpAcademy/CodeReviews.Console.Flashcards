using System.ComponentModel.DataAnnotations;

namespace Flashcards.Models;

public class Flashcard
{
    public int Id { get; set; }

    [Required] [MaxLength(250)] public string Front { get; set; } = string.Empty;

    [Required] [MaxLength(250)] public string Back { get; set; } = string.Empty;

    public int StackId { get; set; }

    public Stack Stack { get; set; } = null!;
}