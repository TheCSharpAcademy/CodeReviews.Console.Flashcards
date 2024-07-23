using Flashcards.Data.Managers;
using Flashcards.Models;

namespace Flashcards.Controllers;

/// <summary>
/// Controller for all interactions between the StudySessionReport model and entity.
/// </summary>
public class StudySessionReportController
{
    #region Fields

    private readonly SqlDataManager _dataManager;

    #endregion
    #region Constructors

    public StudySessionReportController(string connectionString)
    {
        _dataManager = new SqlDataManager(connectionString);
    }

    #endregion
    #region Methods

    public IReadOnlyList<StudySessionReportDto> GetAverageStudySessionScoreReportByYear(DateTime dateTime)
    {
        return _dataManager.GetAverageStudySessionScoreReportByYear(dateTime).Select(x => new StudySessionReportDto(x)).ToList();
    }

    public IReadOnlyList<StudySessionReportDto> GetTotalStudySessionsReportByYear(DateTime dateTime)
    {
        return _dataManager.GetTotalStudySessionsReportByYear(dateTime).Select(x => new StudySessionReportDto(x)).ToList();
    }

    #endregion
}
