namespace Flashcards.Models;

public class Stack
{
    public int StackID { get; set; }
    public string StackName { get; set; }

    public Stack() { }

    public Stack(string stackName)
    {
        StackName = stackName;
    }   
}
