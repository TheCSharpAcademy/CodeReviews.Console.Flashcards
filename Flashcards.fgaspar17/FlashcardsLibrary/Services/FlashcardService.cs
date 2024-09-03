namespace FlashcardsLibrary;
public static class FlashcardService
{
    public static List<FlashcardDto> GetFlashcards(int stackId)
    {
        List<Flashcard> flashcards = FlashcardController.GetFlashcardsByStackId(stackId);
        return flashcards.Select(flashcard => FlashcardMapper.MapToDto(flashcard)).ToList();
    }
}