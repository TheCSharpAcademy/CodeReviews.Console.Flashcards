using FlashcardApp.Core.Models;
using FlashcardApp.Core.DTOs;
namespace FlashcardApp.Core.Services.Interfaces;

public interface IStudySessionService
{
    Task<Result<string>> AddStudySession(StudySession studySession);
    Task<Result<IEnumerable<StudySession>>> GetStudySessionsByStackName(string name);
    Task<Result<List<ReportingDto>>> GetStudySessionsReport(string year);
}
