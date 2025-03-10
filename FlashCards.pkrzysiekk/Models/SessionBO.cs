namespace FlashCards.Models
{
    public class SessionBO
    {
        private int? Id { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public DateTime Date { get; set; }
        public int StackId { get; set; }
    }
}