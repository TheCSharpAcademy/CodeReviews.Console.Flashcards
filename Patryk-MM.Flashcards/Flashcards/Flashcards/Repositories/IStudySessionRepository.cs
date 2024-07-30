using Flashcards.DTOs;
using Flashcards.Models;

namespace Flashcards.Repositories;
public interface IStudySessionRepository : IBaseRepository<StudySession> {
    public Task<List<ReportDTO>> GetMonthlyAveragesAsync(int year);
    public Task<List<ReportDTO>> GetSumOfSessionsAsync(int year);
}
