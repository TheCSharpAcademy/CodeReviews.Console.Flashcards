namespace cacheMe512.Flashcards.DTOs;

public class FlashcardDTO
{
    public int Id { get; }
    public string Question { get; }
    public string Answer { get; }

    public FlashcardDTO(int id, string question, string answer)
    {
        Id = id;
        Question = question;
        Answer = answer;
    }
}
