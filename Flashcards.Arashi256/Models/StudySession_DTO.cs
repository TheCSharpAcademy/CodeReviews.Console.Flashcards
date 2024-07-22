namespace Flashcards.Arashi256.Models
{
    internal class StudySession_DTO
    {
        public int? Id = null;
        public int? DisplayId = null;
        public int StackId;
        public int TotalCards = 0;
        public int Score = 0;
        public DateTime DateStudied = DateTime.Now;
        public string Subject;
    }
}