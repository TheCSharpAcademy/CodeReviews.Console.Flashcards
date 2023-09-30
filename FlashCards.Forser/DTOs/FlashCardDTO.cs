namespace FlashCards.Forser.DTOs
{
    public class FlashCardDTO
    {
        public int? CardId { get; set; }
        public string? Front { get; set; }
        public string? Back { get; set; }

        public FlashCardDTO(int? cardId, string? front, string? back) 
        {
            CardId = cardId;
            Front = front;
            Back = back;
        }
    }
}
