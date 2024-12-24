using Flashcards.Models;

namespace Flashcards.Mappers;

class FlashcardMapper
{
    internal FlashcardsDTO MapFlashcardToDTO(int id, FlashcardsModel record, FlashcardsModel test, string? stackName)
    {
        return new FlashcardsDTO
        {
            DisplayId = id,
            Front = record.Front,
            Back = test.Back,
            StackName = stackName
        };
    }
}