namespace Flashcards.Models
{
    internal class FlashcardModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        private int StackId { get; set; }
    }
}
