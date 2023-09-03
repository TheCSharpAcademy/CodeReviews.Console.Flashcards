namespace Flashcards.w0lvesvvv.Models
{
    public class StudySession
    {
        internal int StudySessionId { get; set; }
        public int StudySessionStackId { get; set; }
        public DateTime StudySessionDate { get; set; }
        public int StudySessionPoints { get; set; }
        public int StudySessionMaxPoints { get; set; }
    }
}
