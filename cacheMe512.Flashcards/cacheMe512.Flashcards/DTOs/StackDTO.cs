
namespace cacheMe512.Flashcards.DTOs;

public class StackDTO
{
    public int Id { get; set; }
    public string Name { get; }
    public IReadOnlyList<FlashcardDTO> Flashcards { get; }
    public int Position { get; }

    public StackDTO(string name, List<FlashcardDTO> flashcards = null, int position = 0)
    {
        Name = name;
        Flashcards = flashcards?.AsReadOnly() ?? new List<FlashcardDTO>().AsReadOnly();
        Position = position;
    }
}
