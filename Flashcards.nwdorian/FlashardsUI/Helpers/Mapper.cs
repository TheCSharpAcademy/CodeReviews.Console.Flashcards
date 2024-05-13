using FlashcardsLibrary.Models;

namespace FlashardsUI.Helpers;
internal static class Mapper
{
    public static StackDTO ToStackDTO(this Stack stack)
    {
        return new StackDTO
        {
            Name = stack.Name
        };
    }

    public static FlashcardDTO ToFlashcardDTO(this Flashcard flashcard)
    {
        return new FlashcardDTO
        {
            Question = flashcard.Question,
            Answer = flashcard.Answer
        };
    }

    public static StudySessionDTO ToStudySessionDTO(this StudySession studysession)
    {
        return new StudySessionDTO
        {
            Name = studysession.Stack.Name,
            Date = studysession.Date,
            Score = studysession.Score
        };
    }
}
