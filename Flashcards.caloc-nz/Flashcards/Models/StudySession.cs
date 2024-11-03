using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.Models;

public class StudySession
{
    public int Id { get; set; }

    // The date of the study session
    public DateTime Date { get; set; }

    // Score, validated to be within 0-100
    [Range(0, 100)] public int Score { get; set; }

    // Foreign key to the Stack entity
    [ForeignKey("Stack")] public int StackId { get; set; }

    // Optional navigation property for related Stack entity
    public Stack? Stack { get; set; }
}