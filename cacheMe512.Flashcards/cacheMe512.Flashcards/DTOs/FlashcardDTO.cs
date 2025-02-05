namespace cacheMe512.Flashcards.DTOs;

public class FlashcardDto
{
    public int Id { get; }
    public string Question { get; }
    public string Answer { get; }
    public int Position { get; }

    public FlashcardDto(int id, string question, string answer, int position)
    {
        Id = id;
        Question = question;
        Answer = answer;
        Position = position;
    }
}
