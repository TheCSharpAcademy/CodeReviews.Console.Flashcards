namespace Flashcards.UndercoverDev.Models
{
    public class Stack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Collection of related Flashcards
        public List<Flashcard> Flashcards { get; set; }

        public Stack()
        {
            Flashcards = [];
        }
    }
}