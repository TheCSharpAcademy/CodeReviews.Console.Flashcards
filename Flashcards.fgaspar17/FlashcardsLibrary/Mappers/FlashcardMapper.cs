namespace FlashcardsLibrary;
public static class FlashcardMapper
{
    public static FlashcardDTO MapToDTO(Flashcard flashcard)
    {
        return new FlashcardDTO
        {
            Id = flashcard.OrderId,
            Question = flashcard.Question,
            Answer = flashcard.Answer,
        };
    }
}