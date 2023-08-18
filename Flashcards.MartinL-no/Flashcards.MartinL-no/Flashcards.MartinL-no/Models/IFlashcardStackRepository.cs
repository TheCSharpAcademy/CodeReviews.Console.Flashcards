namespace Flashcards.MartinL_no.Models;

internal interface IFlashcardStackRepository
{
    public List<FlashcardStack> GetStacks();

    public FlashcardStack GetStackByName(string name);

    public bool InsertStack(FlashcardStack stack);

    public bool InsertFlashcard(Flashcard flashcard);

    public bool UpdateFlashcard(Flashcard flashCard);

    public bool DeleteStack(int id);

    public bool DeleteFlashCard(int id);
}