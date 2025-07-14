
namespace FlashCard.SheheryarRaza.Entities
{
    public class StudySession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
        public int StackId { get; set; }
        public Stacks Stack { get; set; } = null!;
        public TimeSpan Duration => EndTime - StartTime;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }

    }
}
