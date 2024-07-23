using Flashcards.Data.Entities;

namespace Flashcards.Models;

/// <summary>
/// Presentation version of the StudySessionReport object.
/// </summary>
public class StudySessionReportDto
{
    #region Constructors
    
    public StudySessionReportDto(StudySessionReportEntity entity)
    {
        StackName = entity.StackName;
        StudyYear = entity.StudyYear;
        January = entity.January;
        February = entity.February;
        March = entity.March;
        April = entity.April;
        May = entity.May;
        June = entity.June;
        July = entity.July;
        August = entity.August;
        September = entity.September;
        October = entity.October;
        November = entity.November;
        December = entity.December;
    }

    #endregion
    #region Properties

    public string StackName { get; init; }

    public string StudyYear { get; init; }

    public int January { get; init; }

    public int February { get; init; }

    public int March { get; init; }

    public int April { get; init; }

    public int May { get; init; }

    public int June { get; init; }

    public int July { get; init; }

    public int August { get; init; }

    public int September { get; init; }

    public int October { get; init; }

    public int November { get; init; }

    public int December { get; init; }

    #endregion
}
