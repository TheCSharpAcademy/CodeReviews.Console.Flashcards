using FlashCards.kwm0304.Dtos;

namespace FlashCards.kwm0304.Interfaces;

public interface IStudySessionService
{
  Task<List<StudySessionDto>> GetAllStudySessionsAsync();
  Task CreateStudySessionAsync(int stackId, int score);
}