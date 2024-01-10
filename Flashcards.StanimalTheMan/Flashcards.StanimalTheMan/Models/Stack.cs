namespace Flashcards.StanimalTheMan.Models;

internal class Stack
{
    public Stack(int stackId, string stackName)
    {
        StackId = stackId;
        StackName = stackName;
    }

    public int StackId { get; set; }
    public string StackName { get; set; }
}
