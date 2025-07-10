namespace FlashCards.Models
{
    internal class Deck
    {
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal List<Flashcard> Flashcards { get; set; }

        internal Deck()
        {
            Name = string.Empty;
            Flashcards = new List<Flashcard>();
        }

        internal Deck(string name, List<Flashcard> flashcards)
        {
            Name = name;
            Flashcards = flashcards;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

    }
}
