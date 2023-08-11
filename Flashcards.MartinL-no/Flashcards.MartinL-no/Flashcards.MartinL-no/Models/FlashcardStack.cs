namespace Flashcards.MartinL_no.DAL.Models;

internal class FlashcardStack
{
    public readonly int Id;
    public readonly string Name;
    public readonly Stack<string> Flashcards;

    public FlashcardStack(int id, string name, Stack<string> flashcards)
    {
        Id = id;
        Name = name;
        Flashcards = flashcards;
    }
}