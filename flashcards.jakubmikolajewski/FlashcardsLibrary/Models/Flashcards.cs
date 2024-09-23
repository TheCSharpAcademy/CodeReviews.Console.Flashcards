namespace FlashcardsLibrary.Models;

public class Flashcards
{
    public int FlashcardId { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }

    public Flashcards(int flashcardId, string front, string back, int stackId)
    {
        FlashcardId = flashcardId;
        Front = front;
        Back = back;
        StackId = stackId;
    }
    public static List<Flashcards> FlashcardsList
    {
        get => DatabaseQueries.Run.SelectAllFlashcards();
    }
}
