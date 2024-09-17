using System.ComponentModel.DataAnnotations;

namespace Flashcards.Models
{
    public class Stack
    {
        [Key]
        public int StackId { get; set; }
        public string StackName { get; set; }
        public ICollection<Flashcard> Flashcards { get; set; }
        public ICollection<StudySession> StudySessions { get; set; }

    }
}
