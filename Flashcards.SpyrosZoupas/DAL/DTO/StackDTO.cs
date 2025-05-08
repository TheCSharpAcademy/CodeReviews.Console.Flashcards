namespace Flashcards.DAL.DTO;

public class StackDto
{
    public string Name { get; set; }
    public List<FlashcardStackDto> Flashcards { get; set; } = new List<FlashcardStackDto>();
}
