namespace Flashcards;

internal class CardStack
{
    public string StackName { get; set; }
    public int Id { get; set; }
    public static List<CardStack> Stacks { get; set; } = [];
    public List<Card> Cards { get; set; } = [];
    public CardStack(string name)
    {
        StackName = name;
        Stacks.Add(this);
    }

    public CardStack(string StackName, int Id)
    {
        this.StackName = StackName;
        this.Id = Id;
        Stacks.Add(this);
    }
}