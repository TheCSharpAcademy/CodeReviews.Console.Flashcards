using System.Collections.Generic;

namespace Flashcards.Domain.DTO
{
    public class StackDto
    {
        public string Name { get; set; } = string.Empty;
        public List<FlashcardDto> Flashcards { get; set; } = new List<FlashcardDto>(); 
    }
}
