namespace DataAccessLibrary.Models
{
    public class FlashCardsModel
    {
        public int id { get; set; }
        public int StackName { get; set; }
        public int FlashCardId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
