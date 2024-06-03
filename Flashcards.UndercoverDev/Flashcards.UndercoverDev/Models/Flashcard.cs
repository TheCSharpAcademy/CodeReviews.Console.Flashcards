namespace Flashcards.UndercoverDev.Models
{
    public class Flashcard
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        // Navigation property for related Stack
        public Stack Stack { get; set; }
    }
}