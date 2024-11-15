using Flashcards.TwilightSaw.Models;

namespace Flashcards.TwilightSaw.Controller;

public class FlashcardMapper
{
    public static FlashcardDto ConvertToDto(Flashcard flashcard)
    {
        return new FlashcardDto()
        {
            Back = flashcard.Back,
            Front = flashcard.Front,
            Id = flashcard.Id
        };
    }
}