public interface IHandleFlashcards
{
    public void CreateFlashcard(Flashcard flashcard);
    public void DeleteFlashcard(int flashcardId);
    public List<Flashcard> GetFlashcardsByStack(int stackId);
}