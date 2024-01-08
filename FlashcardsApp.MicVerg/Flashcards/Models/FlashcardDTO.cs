namespace Flashcards.Models
{
    internal class FlashcardDTO
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string StackName { get; set; }
    }
}
