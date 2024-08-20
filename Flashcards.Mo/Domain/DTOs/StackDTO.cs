

namespace Flashcards.Domain.DTO
{
    public class StackDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<FlashcardDTO> Flashcards { get; set; } = new List<FlashcardDTO>(); 
    }
}
