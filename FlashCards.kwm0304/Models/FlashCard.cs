namespace FlashCards.kwm0304.Models;

public class FlashCard
{
  public int FlashCardId { get; set; }
  public int StackId { get; set; }
  public Stack? Stack { get; set; }
  public string? Question { get; set; }
  public string? Answer { get; set; }
  public FlashCard(string question, string answer, int stackId)
  {
    Question = question;
    Answer = answer;
    StackId = stackId;
  }
}