namespace Flashcards.Model {
    internal class Cards {

        public Cards( string id, string front, string back, string deckId ) {
            Id = id;
            Front = front;
            Back = back;
            DeckId = deckId;
        }

        public string Id { get; set; }
        public String Front { get; set; }
        public String Back { get; set; }
        public String DeckId { get; set; }
    }
}
