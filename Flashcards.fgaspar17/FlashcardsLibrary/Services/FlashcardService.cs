namespace FlashcardsLibrary;
public static class FlashcardService
{
    public static List<FlashcardDTO> GetFlashcards(int stackId)
    {
        List<Flashcard> flashcards = FlashcardController.GetFlashcardsByStackId(stackId);
        return flashcards.Select(flashcard => FlashcardMapper.MapToDTO(flashcard)).ToList();
    }
}