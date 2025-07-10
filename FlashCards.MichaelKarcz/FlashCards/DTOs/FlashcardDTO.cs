namespace FlashCards.DTOs
{
    internal class FlashcardDTO
    {
        internal int Id { get; set; }
        internal string Front { get; set; }
        internal string Back { get; set; }
        internal int DeckId { get; set; }

        internal FlashcardDTO()
        {
            Front = string.Empty;
            Back = string.Empty;
        }
        
        internal FlashcardDTO(int id, string front, string back, int deckId) 
        {
            Id = id;
            Front = front;
            Back = back;
            DeckId = deckId;

        }

        public override string ToString()
        {
            return $"{Front} / {Back}";
        }

    }
}
