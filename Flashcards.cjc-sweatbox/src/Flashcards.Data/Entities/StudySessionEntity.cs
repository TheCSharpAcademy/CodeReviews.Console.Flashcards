using System.Data;
using Flashcards.Data.Extensions;

namespace Flashcards.Data.Entities;

/// <summary>
/// Database version of the StudySession object.
/// </summary>
public class StudySessionEntity
{
    #region Constructors

    public StudySessionEntity(IDataReader reader)
    {
        Id = reader.GetInt32("Id");
        StackId = reader.GetInt32("StackId");
        DateTime = reader.GetDateTime("DateTime");
        Score = reader.GetInt32("Score");
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public int StackId { get; init; }

    public DateTime DateTime { get; init; }

    public int Score { get; init; }

    #endregion
}
