using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.Dreamfxx.Models;
public class Session
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Stack))]
    public int StackId { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public int CorrectAnswers { get; set; }

    [Required]
    public int WrongAnswers { get; set; }

    public virtual Stack? Stack { get; set; }
    public virtual List<Flashcard> Flashcards { get; set; } = new();

}
