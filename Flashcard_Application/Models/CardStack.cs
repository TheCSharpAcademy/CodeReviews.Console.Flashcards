namespace Flashcards.Models;

public class CardStack
{
    public int StackId { get; set; }
    public string StackName { get; set; }
    public string StackDescription { get; set; }

    public CardStack()
    {

    }

    public CardStack(string stackName, string stackDescription)
    {
        StackName = stackName;
        StackDescription = stackDescription;
    }
}
