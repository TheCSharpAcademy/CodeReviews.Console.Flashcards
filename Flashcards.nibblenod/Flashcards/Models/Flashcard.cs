namespace Flashcards.Models
{
    internal class Flashcard
    {
        public int ID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

        public string StackID { get; set; }

    }
}
