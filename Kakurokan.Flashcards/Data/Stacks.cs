namespace Kakurokan.Flashcards
{
    public class Stacks
    {
        public int StackId { get; private set; }
        public string Name { get; private set; }
        public Stacks(int stackId, string name)
        {
            StackId = stackId;
            Name = name;
        }

        public Stacks(string name)
        {
            Name = name;
        }
    }
}
