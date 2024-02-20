namespace Flashcards.Mateusz_Platek.Models;

public class FlashcardDTO
{
    public string word { get; }
    public string translation { get; }

    public FlashcardDTO(string word, string translation)
    {
        this.word = word;
        this.translation = translation;
    }
}