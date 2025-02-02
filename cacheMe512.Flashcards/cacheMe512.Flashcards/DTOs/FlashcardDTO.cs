namespace cacheMe512.Flashcards.DTOs;

public class FlashcardDTO
{
    public int Id { get; }
    public string Question { get; }
    public string Answer { get; }
    public int Position { get; }

    public FlashcardDTO(int id, string question, string answer, int position)
    {
        Id = id;
        Question = question;
        Answer = answer;
        Position = position;
    }
}
