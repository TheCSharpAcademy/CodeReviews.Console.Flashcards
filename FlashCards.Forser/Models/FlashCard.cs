namespace FlashCards.Forser.Models
{
    public class FlashCard
    {
        public int Id { get; }
        public int CardId { get; set; }
        public int StackId { get; set; }
        public required string Front {  get; set; }
        public required string Back {  get; set; }

        public FlashCard(int cardId, int stackId, string front, string back)
        {
            CardId = cardId;
            StackId = stackId;
            Front = front;
            Back = back;
        }
    }
}