public class FlashcardController
{
    private FlashcardRepository _flashcardRepo;

    public FlashcardController(FlashcardRepository flashcardRepo)
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

    public void DeleteFlashcard(int flashcardId) => _flashcardRepo.DeleteFlashcard(flashcardId);

    public void UpdateFlashcard(Flashcard newFlashcard, Flashcard oldFlashcard)
    {
        if (newFlashcard != oldFlashcard && newFlashcard.Question.Length >= 0 && newFlashcard.Answer.Length >= 0)
        {
            _flashcardRepo.UpdateCard(newFlashcard);
        }
    }

    public StackDto GetFlashcardsByStack(Stack stack)
    {
        return new StackDto()
        {
            Id = stack.Id,
            Name = stack.Name,
            Flashcards = new List<Flashcard>(_flashcardRepo.GetFlashcardsByStack(stack.Id))
        };
    }

    

}