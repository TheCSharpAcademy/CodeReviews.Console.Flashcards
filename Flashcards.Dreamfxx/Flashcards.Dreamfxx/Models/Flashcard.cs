using System.ComponentModel.DataAnnotations.Schema;
namespace Flashcards.Dreamfxx.Models;

public class Flashcard
{
    public int Id { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }

    [ForeignKey("Stack")]
    public int StackId { get; set; }

    public Stack? Stack { get; set; }
}

