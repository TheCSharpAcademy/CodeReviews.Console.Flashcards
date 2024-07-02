namespace Flashcards.ukpagrace.DTO
{
    class FlashcardDto
    {
        public int? Id { get; set; }
        public int FlashcardId { get; set; }
        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;
    }
}