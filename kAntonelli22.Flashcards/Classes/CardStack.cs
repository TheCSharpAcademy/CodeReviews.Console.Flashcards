using System.Data;

namespace Flashcards;

internal class CardStack
{
    public string StackName { get; set; }
    public int StackSize { get; set; }
    public static List<CardStack> Stacks { get; set; } = new List<CardStack>();
    public List<Card> Cards { get; set; } = new List<Card>();
    public CardStack(string name, int size)
    {
        StackName = name;
        StackSize = size;
        Stacks.Add(this);
    }

}