using System.Data;

namespace Flashcards;

internal class CardStack
{
    public string name;
    public int size;
    public static List<CardStack> Stacks { get; set; } = new List<CardStack>();
    public List<Card> Cards { get; set; } = new List<Card>();
    public CardStack(string stackName, int stackSize)
    {
        name = stackName;
        size = stackSize;
        Stacks.Add(this);
    }

}