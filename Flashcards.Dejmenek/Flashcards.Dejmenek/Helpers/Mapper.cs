using Flashcards.Dejmenek.Models;

namespace Flashcards.Dejmenek.Helpers;

public static class Mapper
{
    public static FlashcardDTO ToFlashcardDTO(Flashcard flashcard)
    {
        return new FlashcardDTO
        {
            Id = flashcard.Id,
            Front = flashcard.Front,
            Back = flashcard.Back,
        };
    }

    public static StackDTO ToStackDTO(Stack stack)
    {
        return new StackDTO
        {
            Name = stack.Name,
        };
    }

    public static StudySessionDTO ToStudySessionDTO(StudySession studysession)
    {
        return new StudySessionDTO
        {
            Id = studysession.Id,
            Date = studysession.Date,
            Score = studysession.Score,
        };
    }
}
