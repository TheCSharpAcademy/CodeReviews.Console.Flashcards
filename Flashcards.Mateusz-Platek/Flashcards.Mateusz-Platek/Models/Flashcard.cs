namespace Flashcards.Mateusz_Platek.Models;

public class Flashcard
{
    public int flashcardId { get; set; }
    public int stackId { get; set; }
    public string word { get; set; }
    public string translation { get; set; }

    public Flashcard(int flashcardId, int stackId, string word, string translation)
    {
        this.flashcardId = flashcardId;
        this.stackId = stackId;
        this.word = word;
        this.translation = translation;
    }
}