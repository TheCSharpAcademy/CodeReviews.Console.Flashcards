namespace Flashcards.MartinL_no.Models;

internal class FlashcardStack
{
    public readonly string Name;
    public readonly Stack<string> Flashcards;

    public FlashcardStack(string name, Stack<string> flashcards)
    {
        Name = name;
        Flashcards = flashcards;
    }
}