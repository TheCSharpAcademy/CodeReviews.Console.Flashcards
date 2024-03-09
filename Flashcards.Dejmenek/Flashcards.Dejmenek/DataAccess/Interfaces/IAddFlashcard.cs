namespace Flashcards.Dejmenek.DataAccess.Interfaces;

internal interface IAddFlashcard
{
    void AddFlashcard(int stackId, string front, string back);
}
