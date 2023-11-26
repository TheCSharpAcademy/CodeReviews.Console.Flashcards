namespace FlashCards.Forser.Models
{
    public class FlashCard
    {
        public int Id { get; }
        public int CardId { get; set; }
        public int StackId { get; set; }
        public string Front { get; set; } = string.Empty;
        public string Back { get; set; } = string.Empty;

        public FlashCard() {}
        public FlashCard(int cardId, string front, string back)
        {
            CardId = cardId;
            Front = front;
            Back = back;
        }
        public FlashCard(string front, string back)
        {
            Front = front;
            Back = back;
        }
        public FlashCard(string front, string back, int stackId)
        {
            Front = front;
            Back = back;
            StackId = stackId;
        }
        public FlashCard(int cardId, int stackId, string front, string back)
        {
            CardId = cardId;
            StackId = stackId;
            Front = front;
            Back = back;
        }
    }
}