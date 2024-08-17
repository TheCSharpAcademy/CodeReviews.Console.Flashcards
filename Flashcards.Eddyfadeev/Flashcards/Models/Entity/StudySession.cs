using Flashcards.Extensions;
using Flashcards.Interfaces.Models;

namespace Flashcards.Models.Entity;

/// <summary>
/// Represents a study session.
/// </summary>
internal class StudySession : IStudySession, IDbEntity<IStudySession>
{
    public int Id { get; set; }
    public int? StackId { get; set; }
    public DateTime Date { get; set; }
    public int Questions { get; set; }
    public int CorrectAnswers { get; set; }
    public int Percentage { get; set; }
    public TimeSpan Time { get; set; }
    public string? StackName { get; set; }

    /// <summary>
    /// Maps the <see cref="IStudySession"/> object to a <see cref="StudySessionDto"/> object.
    /// </summary>
    /// <param name="studySession">The study session to map.</param>
    /// <returns>The mapped <see cref="StudySessionDto"/> object.</returns>
    public IStudySession MapToDto() => this.ToDto();
}