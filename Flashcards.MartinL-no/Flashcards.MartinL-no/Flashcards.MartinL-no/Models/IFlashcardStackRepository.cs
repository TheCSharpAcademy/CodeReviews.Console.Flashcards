namespace Flashcards.MartinL_no.Models;

internal interface IFlashcardStackRepository
{
    public List<FlashcardStack> GetStacks();

    public FlashcardStack GetStackByName(string name);

    public bool InsertStack(FlashcardStack stack);

    public bool InsertFlashcard(string text, string stackName);

    public bool DeleteStack(FlashcardStack stack);

    public bool DeleteFlashCard(string text, string stackName);
}