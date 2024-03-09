using flashcards.Fennikko.Models;

namespace flashcards.Fennikko;

public class FlashcardController
{
    public static FlashcardDto MapToDto(Flashcards flashcard)
    {
        return new FlashcardDto
        {
            FlashcardIndex = flashcard.FlashcardIndex,
            CardFront = flashcard.CardFront,
            CardBack = flashcard.CardBack,
        };
    }

    public static List<FlashcardDto> MapToDto(List<Flashcards> flashcards)
    {
        return flashcards.Select(MapToDto).ToList();
    }
}