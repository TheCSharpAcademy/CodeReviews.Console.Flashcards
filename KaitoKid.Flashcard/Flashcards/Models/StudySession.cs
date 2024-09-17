using System.ComponentModel.DataAnnotations;

namespace Flashcards.Models
{
    public class StudySession
    {
        [Key]
        public int SessionId { get; set; }
        public string StackName { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
        public string Duration { get; set; }
        public int StackId { get; set; }
        public Stack Stack { get; set; }
    }
}
