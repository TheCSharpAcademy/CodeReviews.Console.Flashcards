namespace FlashcardApp.Core.DTOs;

public class FlashcardDTO
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public FlashcardDTO(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }

}