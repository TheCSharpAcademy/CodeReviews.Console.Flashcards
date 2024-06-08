namespace Flashcards.UndercoverDev.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public DateTime SessionDate { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
    }
}