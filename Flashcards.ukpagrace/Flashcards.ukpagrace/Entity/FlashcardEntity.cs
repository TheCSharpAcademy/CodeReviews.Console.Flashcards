namespace Flashcards.ukpagrace.Entity
{
    class FlashcardEntity
    {
        public int Id { get; set; }
        public int StackId { get; set; }

        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty ;
    }
}