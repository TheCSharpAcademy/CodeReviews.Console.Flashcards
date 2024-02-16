using FlashCards.Cactus.DataModel;

namespace FlashCards.Cactus.Service;
public class StackService
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

