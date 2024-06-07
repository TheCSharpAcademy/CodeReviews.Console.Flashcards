namespace Flashcards.UndercoverDev.Models
{
    public class Flashcard
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";

        public static FlashcardDTO ToFlashcardDTO(Flashcard flashcard)
        {
            return new FlashcardDTO
            {
                Question = flashcard.Question,
                Answer = flashcard.Answer
            };
        }
    }

    public class FlashcardDTO
    {
        public int StackId { get; set; }
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
    }
}