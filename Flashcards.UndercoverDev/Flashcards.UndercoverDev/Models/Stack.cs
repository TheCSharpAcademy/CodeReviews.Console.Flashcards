namespace Flashcards.UndercoverDev.Models
{
    public class Stack
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        // Collection of related Flashcards
        public List<Flashcard> Flashcards { get; set; } = [];
    }

    public class StackDTO
    {
        public string Name { get; set; } = "";
        public List<FlashcardDTO> Flashcards { get; set; } = [];
    }
}