namespace Flashcards
{
    class FlashcardModel
    {
        public int FlashcardId { get; set; }
        public int StackId { get; set; }
        public string? Front { get; set; }
        public string? Back { get; set; }

    }
}