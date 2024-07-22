namespace Flashcards.Arashi256.Models
{
    internal class StudySession
    {
        public int? Id = null;
        public int StackId;
        public int TotalCards;
        public int Score;
        public DateTime DateStudied = DateTime.Now;
    }
}