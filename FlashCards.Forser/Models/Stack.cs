namespace FlashCards.Forser.Models
{
    public class Stack
    {
        public int StackId { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<FlashCard> FlashCards { get; set; } = new List<FlashCard>();

        public Stack() { }
        public Stack(string name)
        {
            Name = name;
        }
        public Stack(int stackId, string name) 
        {
            StackId = stackId;
            Name = name;
        }
        public Stack(int stackId, string name, List<FlashCard>? flashCards)
        {
            StackId = stackId;
            Name = name;
            FlashCards = flashCards;
        }
    }
}
