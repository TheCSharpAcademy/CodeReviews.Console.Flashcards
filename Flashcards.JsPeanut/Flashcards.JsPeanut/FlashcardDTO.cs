namespace Flashcards.JsPeanut
{
    class FlashcardDTO
    {
        public int FlashcardId { get; set; }
        public string Question { get; set; }

        public string Answer { get; set; }

        public int? Difficulty { get; set; }
    }
}
