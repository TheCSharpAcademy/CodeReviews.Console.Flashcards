namespace DatabaseLibrary.Models;

public class FlashcardDTO
{
  public int Display_Id { get; set; }
  public string Question { get; set; }
  public string Answer { get; set; }
  public string Stack_Name { get; set; }

  public FlashcardDTO() { }

  public FlashcardDTO(int id, string question, string answer, Stack stack)
  {
    Display_Id = id;
    Question = question;
    Answer = answer;
    Stack_Name = stack.Name;
  }
}