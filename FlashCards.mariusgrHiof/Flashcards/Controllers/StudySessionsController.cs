using Flashcards.Data;
using Flashcards.Models;

namespace Flashcards.Controllers;

public class StudySessionsController
{
    private readonly DataAccess _dataAccess;

    public StudySessionsController(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<List<StudySession>> GetAllStudySessionsAsync()
    {
        return await _dataAccess.GetAllStudySessionsAsync();
    }

    public async Task<List<StudySession>> GetAllStudySessionsByStackIdAsync(int stackId)
    {
        var studySessions = await _dataAccess.GetStudySessionsByStackIdAsync(stackId);

        if (studySessions == null) return null;

        return studySessions;
    }

    public async Task<StudySession> GetStudySessionByIdAsync(int studyId)
    {
        var studySession = await _dataAccess.GetStudySessionByIdAsync(studyId);

        if (studySession == null)
        {
            return null;
        }

        return studySession;
    }

    public async Task<StudySession> AddNewStudySessionAsync(StudySession studySession)
    {
        if (studySession == null) return null;

        var newStudySession = await _dataAccess.AddNewStudySessionAsync(studySession);
        if (newStudySession == null) return null;

        return newStudySession;
    }
}