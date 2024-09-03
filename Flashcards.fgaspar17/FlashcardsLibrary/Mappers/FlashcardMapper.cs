namespace FlashcardsLibrary;
public static class FlashcardMapper
{
    public static FlashcardDto MapToDto(Flashcard flashcard)
    {
        return new FlashcardDto
        {
            Id = flashcard.OrderId,
            Question = flashcard.Question,
            Answer = flashcard.Answer,
        };
    }
}