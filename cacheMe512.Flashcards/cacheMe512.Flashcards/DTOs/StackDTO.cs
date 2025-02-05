namespace cacheMe512.Flashcards.DTOs;

public class StackDto
{
    public int Id { get; set; }
    public string Name { get; }
    public IReadOnlyList<FlashcardDto> Flashcards { get; }
    public int Position { get; }

    public StackDto(int id, string name, List<FlashcardDto> flashcards = null, int position = 0)
    {
        Id = id;
        Name = name;
        Flashcards = flashcards?.AsReadOnly() ?? new List<FlashcardDto>().AsReadOnly();
        Position = position;
    }
}
