namespace Flashcards.Domain.Entities
{
    public class Stack
    {
        public int Id { get; set; }
        
        
        public string Name { get; set; } = string.Empty;
        public ICollection<Flashcard> Flashcards { get; set; }
        public ICollection<StudySession> StudySessions { get; set; }

        public Stack()
        {
            Flashcards = new List<Flashcard>();
            StudySessions = new List<StudySession>();
        }
    }
}
