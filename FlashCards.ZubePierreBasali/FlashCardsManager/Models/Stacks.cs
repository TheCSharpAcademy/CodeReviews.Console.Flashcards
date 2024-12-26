namespace FlashCards.FlashCardsManager.Models
{
    internal class Stacks
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int NumberOfCards { get; set; }
        public List<FlashCard>? FlashCards { get; set; }
    }
}
