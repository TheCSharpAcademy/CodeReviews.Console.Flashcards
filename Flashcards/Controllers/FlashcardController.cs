public class FlashcardController : IHandleFlashcards
{
    private IHandleFlashcards _flashcardRepo;

    public FlashcardController(IHandleFlashcards flashcardRepo)
    {
        _flashcardRepo = flashcardRepo;
    }

    public void CreateFlashcard(Flashcard flashcard)
    {
        if (flashcard.Id != 0 && flashcard.Question.Length != 0 && flashcard.Answer.Length != 0)
        {
            Console.WriteLine("Data was incorrect for creating the flashcard.");
        }
        else
        {
            _flashcardRepo.CreateFlashcard(flashcard);
        }
    }

    public void DeleteFlashcard(int flashcardId)
    {
        throw new NotImplementedException();
    }

    public List<Flashcard> GetFlashcardsByStack(int stackId)
    {
        throw new NotImplementedException();
    }
}