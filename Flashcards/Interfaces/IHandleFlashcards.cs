public interface IHandleFlashcards
{
    public void CreateFlashcard(int stackId, string question, string answer);
    public void DeleteFlashcard(int flashcardId);
    public List<Flashcard> GetFlashcardsByStack(int stackId);
}