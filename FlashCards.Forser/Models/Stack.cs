namespace FlashCards.Forser.Models
{
    public class Stack
    {
        public int StackId { get; set; }
        public required string Name { get; set; }

        public Stack(string name)
        {
            Name = name;
        }
    }
}
