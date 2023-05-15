namespace Flashcards.Model {
    internal class CardDto {

        public CardDto( string id, string cardFront, string cardBack ) {
            Id = id;
            Front = cardFront;
            Back = cardBack;
        }
        public string Id { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
