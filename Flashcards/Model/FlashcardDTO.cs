namespace Flashcards.Model
{
    public class FlashcardDTO
    {
        public int FlashcardId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public FlashcardDTO() { }

        public FlashcardDTO(int flashcardId, string? question, string? answer)
        {
            FlashcardId = flashcardId;
            Question = question;
            Answer = answer;
        }
    }
}
