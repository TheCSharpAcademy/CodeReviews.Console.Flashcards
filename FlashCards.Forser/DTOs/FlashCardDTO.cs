namespace FlashCards.Forser.DTOs
{
    public class FlashCardDto
    {
        public int? CardId { get; set; }
        public string? Front { get; set; }
        public string? Back { get; set; }

        public FlashCardDto(int? cardId, string? front, string? back) 
        {
            CardId = cardId;
            Front = front;
            Back = back;
        }
    }
}
