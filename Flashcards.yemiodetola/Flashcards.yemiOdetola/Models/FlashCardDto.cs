namespace Flashcards.yemiOdetola.Models;

public class FlashCardDto
{
  public string Word { get; set; }
  public string Category { get; set; }

  public FlashCardDto(string word, string category)
  {
    Word = word;
    Category = category;
  }
}