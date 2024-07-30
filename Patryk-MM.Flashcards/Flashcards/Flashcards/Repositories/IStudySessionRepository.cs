using Flashcards.DTOs;
using Flashcards.Models;

namespace Flashcards.Repositories;
public interface IStudySessionRepository : IBaseRepository<StudySession> {
    public Task<List<ReportDto>> GetMonthlyAveragesAsync(int year);
    public Task<List<ReportDto>> GetSumOfSessionsAsync(int year);
}
