namespace Flashcards.Models;

public class CardDTO(string front, string back, string stackName)
{
    public string Front { get; } = front;
    public string Back { get; } = back;
    public string StackName { get; set; } = stackName;
}