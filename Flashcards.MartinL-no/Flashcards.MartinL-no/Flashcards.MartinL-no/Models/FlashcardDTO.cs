namespace Flashcards.MartinL_no.Models;

internal class FlashcardDto
{
    public int Id;
    // Index (plus one) of flashcard in stack
    public int ViewId;
    public string Front;
    public string Back;
}
