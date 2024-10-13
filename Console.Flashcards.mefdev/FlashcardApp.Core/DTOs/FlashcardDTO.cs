namespace FlashcardApp.Core.DTOs;

public class FlashcardDto
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public FlashcardDto(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }
}
