using Flashcards.Data.Entities;

namespace Flashcards.Models;

/// <summary>
/// Presentation version of the StudySession object.
/// </summary>
public class StudySessionDto
{
    #region Constructors

    public StudySessionDto(StudySessionEntity entity)
    {
        Id = entity.Id;
        StackId = entity.StackId;
        DateTime = entity.DateTime;
        Score = entity.Score;
    }

    public StudySessionDto(int stackId, DateTime dateTime, int score)
    {
        StackId = stackId;
        DateTime = dateTime;
        Score = score;
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public int StackId { get; init; }

    public DateTime DateTime { get; init; }

    public int Score { get; init; }

    #endregion
}
