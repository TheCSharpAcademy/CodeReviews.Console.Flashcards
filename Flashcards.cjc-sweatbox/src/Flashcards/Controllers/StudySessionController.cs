using Flashcards.Data.Managers;
using Flashcards.Models;

namespace Flashcards.Controllers;

/// <summary>
/// Controller for all interactions between the StudySession model and entity.
/// </summary>
public class StudySessionController
{
    #region Fields

    private readonly SqlDataManager _dataManager;

    #endregion
    #region Constructors
    
    public StudySessionController(string connectionString)
    {
        _dataManager = new SqlDataManager(connectionString);
    }

    #endregion
    #region Methods

    public void AddStudySession(int stackId, int score)
    {
        _dataManager.AddStudySession(stackId, DateTime.Now, score);
    }

    public void AddStudySession(StudySessionDto studySession)
    {
        _dataManager.AddStudySession(studySession.StackId, studySession.DateTime, studySession.Score);
    }

    public IReadOnlyList<StudySessionDto> GetStudySessions()
    {
        return _dataManager.GetStudySessions().Select(x => new StudySessionDto(x)).ToList();
    }

    #endregion
}
