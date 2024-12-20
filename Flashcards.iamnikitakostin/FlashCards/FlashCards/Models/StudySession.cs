namespace FlashCards.Models
{
    internal class StudySession
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public virtual Stack Stack { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Score { get; set; }
    }
}
