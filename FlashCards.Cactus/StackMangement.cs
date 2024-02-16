using FlashCards.Cactus.DataModel;

namespace FlashCards.Cactus;
public class StackMangement
{
    public List<Stack> Stacks { get; set; }

    public void ShowStacks()
    {
        Console.WriteLine("Show all stacks.");
    }

    public void AddStack()
    {
        Console.WriteLine("Add a new stack.");
    }

    public void DeleteStack()
    {
        Console.WriteLine("Delete a stack.");
    }
}

