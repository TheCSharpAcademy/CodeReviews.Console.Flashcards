namespace Flashcards;

class Stacks
{
    public int StackID;
    public string StackName;

    public Stacks(string stackName, int stackID = 0)
    {
        StackID = stackID;
        StackName = stackName;
    }
}