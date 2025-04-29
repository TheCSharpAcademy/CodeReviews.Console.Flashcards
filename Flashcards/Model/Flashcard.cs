namespace Flashcards.Model
{
    public class Flashcard
    {
        public int FlashcardId { get; set; }

        public int StackId { get; set; }

        public string? Question { get; set; }

        public string? Answer { get; set; }

        public Flashcard() { }

        public Flashcard(int flashcardId, int stackId, string? question, string? answer)
        {
            FlashcardId = flashcardId;
            StackId = stackId;
            Question = question;
            Answer = answer;
        }
    }
}
