namespace Flashcards.yemiOdetola.Models;

public class FlashCard
{
  public int Id { get; set; }
  public int StackId { get; set; }
  public string Word { get; set; }
  public string Category { get; set; }

  public FlashCard(int id, int stackId, string word, string category)
  {
    Id = id;
    StackId = stackId;
    Word = word;
    Category = category;
  }
}