using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetMAUI.Flashcards.Models;
public class StudySession
{
    [Key]
    public int Id { get; set; }
    public DateTime DateStudied { get; set; }
    public int Score { get; set; }
    [ForeignKey(nameof(StackId))]
    public int StackId { get; set; }
    public Stack Stack { get; set; } = null!;
}

