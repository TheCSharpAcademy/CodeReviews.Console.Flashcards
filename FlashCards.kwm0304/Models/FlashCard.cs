namespace FlashCards.kwm0304.Models;

public class FlashCard
{
  public int FlashCardId { get; set; }
  public int StackId { get; set; }
  public Stack? Stack { get; set; }
  public string Question { get; set; }
  public string Answer { get; set; }
  public FlashCard(string question, string answer, int stackId)
  {
    Question = question ?? throw new ArgumentNullException(nameof(question));
    Answer = answer ?? throw new ArgumentNullException(nameof(answer));
    StackId = stackId;
  }
}