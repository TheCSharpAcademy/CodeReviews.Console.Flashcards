using cacheMe512.Flashcards.Models;

namespace cacheMe512.Flashcards.DTOs;

public class StackDTO
{
    public string Name { get; }
    public IReadOnlyList<FlashcardDTO> Flashcards { get; }

    public StackDTO(string name, List<FlashcardDTO> flashcards = null)
    {
        Name = name;
        Flashcards = flashcards?.AsReadOnly() ?? new List<FlashcardDTO>().AsReadOnly();
    }
}
