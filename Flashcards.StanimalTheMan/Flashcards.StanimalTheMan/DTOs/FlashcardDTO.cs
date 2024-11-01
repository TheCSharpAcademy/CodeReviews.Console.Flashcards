namespace Flashcards.StanimalTheMan.DTOs
{
    internal class FlashcardDto
    {
        public FlashcardDto(long flashcardSequentialId, int flashcardId, string front, string back)
        {
            FlashcardSequentialId = flashcardSequentialId;
            FlashcardId = flashcardId;
            Front = front;
            Back = back;
        }

        public long FlashcardSequentialId { get; set; }
        public int FlashcardId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
