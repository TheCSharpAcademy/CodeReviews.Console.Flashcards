using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services.Interfaces;

public interface IStudySessionService
{
    Task<Result<string>> AddStudySession(StudySession studySession);
    Task<Result<IEnumerable<StudySession>>> GetStudySessionsByStackName(string name);
}