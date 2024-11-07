using Flashcards.TwilightSaw.Models;

namespace Flashcards.TwilightSaw.Controller;

public class FlashcardMapper
{
    public static FlashcardDTO ConvertToDto(Flashcard flashcard)
    {
        return new FlashcardDTO()
        {
            Back = flashcard.Back,
            Front = flashcard.Front,
            Id = flashcard.Id
        };
    }
}