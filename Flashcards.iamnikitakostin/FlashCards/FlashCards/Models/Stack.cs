namespace FlashCards.Models
{
    internal class Stack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
        public virtual ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
    }
}
