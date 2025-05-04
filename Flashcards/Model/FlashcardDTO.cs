namespace Flashcards.Model
{
    public class FlashcardDto
    {
        public int FlashcardId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public FlashcardDto() { }

        public FlashcardDto(int flashcardId, string? question, string? answer)
        {
            FlashcardId = flashcardId;
            Question = question;
            Answer = answer;
        }
    }
}
