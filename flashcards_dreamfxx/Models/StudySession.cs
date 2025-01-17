using System.ComponentModel.DataAnnotations.Schema;
using Flashcards.DreamFXX.Services;
namespace Flashcards.DreamFXX.Models;

public class StudySession
{
    public int Id { get; set; }

    [ForeignKey("CardStackId")]
    public int CardStackId { get; set; }
    public List<Card>? Cards { get; set; }
    public DateTime EndTime { get; set; }
    public int CorrectAnswers { get; set; }
    public int WrongAnswers { get; set; }
    public StackofCards StackofCardsList { get; set; }
}
