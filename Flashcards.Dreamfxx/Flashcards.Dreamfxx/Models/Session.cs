using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.Dreamfxx.Models;
public class Session
{
    public int Id { get; set; }
    [ForeignKey("StackId")]
    public int StackId { get; set; }
    public List<Flashcard> Flashcards { get; set; }
    public DateTime EndTime { get; set; }
    public int CorrectAnswers { get; set; }
    public int WrongAnswers { get; set; }
    public Stack Stack { get; set; }
}
