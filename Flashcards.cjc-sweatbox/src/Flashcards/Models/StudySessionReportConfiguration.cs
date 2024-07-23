using Flashcards.Enums;

namespace Flashcards.Models;

/// <summary>
/// Controls the parameters used when creating a StudySession report.
/// </summary>
public class StudySessionReportConfiguration
{
    #region Properties

    public StudySessionReportType Type { get; set; }

    public DateTime Date { get; set; }

    #endregion
}
