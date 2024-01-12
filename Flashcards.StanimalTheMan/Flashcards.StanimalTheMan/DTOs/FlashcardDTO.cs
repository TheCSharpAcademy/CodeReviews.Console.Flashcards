namespace Flashcards.StanimalTheMan.DTOs
{
    internal class FlashcardDTO
    {
        public FlashcardDTO(int flashcardId, string front, string back)
        {
            FlashcardId = flashcardId;
            Front = front;
            Back = back;
        }

        public int FlashcardId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
