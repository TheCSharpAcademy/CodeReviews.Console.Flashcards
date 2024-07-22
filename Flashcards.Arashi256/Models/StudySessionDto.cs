namespace Flashcards.Arashi256.Models
{
    internal class StudySessionDto
    {
        public int? Id = null;
        public int? DisplayId = null;
        public int StackId;
        public int TotalCards;
        public int Score;
        public DateTime DateStudied = DateTime.Now;
        public string Subject;
    }
}