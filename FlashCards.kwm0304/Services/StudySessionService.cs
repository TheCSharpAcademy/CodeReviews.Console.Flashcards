using FlashCards.kwm0304.Dtos;
using FlashCards.kwm0304.Interfaces;

namespace FlashCards.kwm0304.Services;

public class StudySessionService : IStudySessionService
{
    public Task CreateStudySessionAsync(int stackId, int score)
    {
        throw new NotImplementedException();
    }

    public Task<List<StudySessionDto>> GetAllStudySessionsAsync()
    {
        throw new NotImplementedException();
    }

}
