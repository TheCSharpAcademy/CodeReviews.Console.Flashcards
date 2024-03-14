namespace Flashcards.DTOs;

public class FlashcardDto(string flashcardTitle, string stackName)
{
    public string FlashcardTitle { get; set; } = flashcardTitle;
    public string StackName { get; set; } = stackName;
}