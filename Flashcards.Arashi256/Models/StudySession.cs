namespace Flashcards.Arashi256.Models
{
    internal class StudySession
    {
        public int? Id = null;
        public int StackId = 0;
        public int TotalCards = 0;
        public int Score = 0;
        public DateTime DateStudied = DateTime.Now;
    }
}