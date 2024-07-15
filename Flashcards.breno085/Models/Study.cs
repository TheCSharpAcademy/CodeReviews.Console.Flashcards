namespace flashcards.Models
{
    public class Study
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        
        public int Score { get; set; }

        public int StackId { get; set; }
    }
}