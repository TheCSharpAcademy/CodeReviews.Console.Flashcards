

namespace Flashcards.Domain.DTO
{
    public class FlashcardDto
    {
        public int FlashcardNumber { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
