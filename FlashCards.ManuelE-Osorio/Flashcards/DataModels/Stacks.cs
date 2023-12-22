namespace Flashcards;

class Stacks(string stackName, int stackID = 0)
{
    public int StackID = stackID;
    public string StackName = stackName;

     
    public static Stacks FromCSV(string stackLine)
    {
        string[] data = stackLine.Split(',');
        Stacks stack = new(data[0]);
        return stack;
    }
}