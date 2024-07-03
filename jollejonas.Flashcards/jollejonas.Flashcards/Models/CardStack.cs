namespace jollejonas.Flashcards.Models
{
    public class CardStack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Card> Cards { get; set; }
    }
}
