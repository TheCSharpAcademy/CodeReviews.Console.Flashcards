namespace Flashcards.K_MYR.Models
{
    internal class Flashcard
    {
        public int FlashcardId { get; set; }

        public int StackId { get; set; }

        public string FrontText { get; set; }

        public string BackText { get; set; }

        public DateTime Created { get; set; }

        internal void Delete()
        {
            SQLController.DeleteFlashcard(FlashcardId);
        }

        internal void UpdateBackText(string newText)
        {
            SQLController.UpdateFlashcard($"BackText = '{newText}'", FlashcardId);
            BackText = newText;
        }

        internal void UpdateFrontText(string newText)
        {
            SQLController.UpdateFlashcard($"FrontText = '{newText}'", FlashcardId);
            FrontText = newText;
        }


    }

    internal class FlashcardDTO
    {
        public int Row { get; set; }

        public string FrontText { get; set; }

        public string BackText { get; set; }

        public DateTime Created { get; set; }
    }
}
