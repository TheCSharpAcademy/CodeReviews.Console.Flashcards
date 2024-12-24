using Flashcards.Models;

namespace Flashcards.Mappers;

class StudySessionMapper
{
    internal StudySessionDTO MapFlashcardToDTO(StudySession date, StudySession score, string? stackName)
    {
        return new StudySessionDTO
        {
            Date = date.Date,
            Score = score.Score,
            StackName = stackName
        };
    }
}