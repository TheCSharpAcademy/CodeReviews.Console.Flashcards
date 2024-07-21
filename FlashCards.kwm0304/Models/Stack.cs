namespace FlashCards.kwm0304.Models;

public class Stack
{
  public int StackId { get; set; }
  public string? StackName { get; set; }
  public List<FlashCard> Flashcards { get; set; }
  public List<StudySession> StudySessions { get; set; }

  public Stack(string name)
  {
    StackName = name;
    Flashcards = [];
    StudySessions = [];
  }
}