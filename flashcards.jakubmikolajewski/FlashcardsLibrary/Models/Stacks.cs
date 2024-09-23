namespace FlashcardsLibrary.Models;

public class Stacks
{
    public int StackId { get; set; }
    public string StackName { get; set; }
    public Stacks(int stackId, string stackName)
    {
        StackId = stackId;
        StackName = stackName;
    }
    public static List<Stacks> StackList
    {
        get => DatabaseQueries.Run.SelectAllStacks();
    }
}

