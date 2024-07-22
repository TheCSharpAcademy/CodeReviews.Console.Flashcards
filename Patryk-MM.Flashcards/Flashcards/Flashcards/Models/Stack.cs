namespace Flashcards.Models {
    public class Stack {
        public int StackId { get; set; }
        public string Name { get; set; }
        public ICollection<Flashcard> Flashcards { get; set; }
    }
}
