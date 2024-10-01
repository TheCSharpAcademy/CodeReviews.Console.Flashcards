namespace Flashcards.Models;

public class Flashcard
{
    public int CardId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackId { get; set; }

    public Flashcard()
    {
    }

    public Flashcard(string question, string answer, int stackId)
    {
        Question = question;
        Answer = answer;
        StackId = stackId;
    }
}
