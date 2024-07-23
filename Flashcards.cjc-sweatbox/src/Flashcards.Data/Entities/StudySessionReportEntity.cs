using System.Data;
using Flashcards.Data.Extensions;

namespace Flashcards.Data.Entities;

/// <summary>
/// Database version of the StudySessionReport object.
/// </summary>
public class StudySessionReportEntity
{
    #region Constructors

    public StudySessionReportEntity(IDataReader reader)
    {
        StackName = reader.GetString("StackName");
        StudyYear = reader.GetString("StudyYear");
        January = reader.GetInt32("January");
        February = reader.GetInt32("February");
        March = reader.GetInt32("March");
        April = reader.GetInt32("April");
        May = reader.GetInt32("May");
        June = reader.GetInt32("June");
        July = reader.GetInt32("July");
        August = reader.GetInt32("August");
        September = reader.GetInt32("September");
        October = reader.GetInt32("October");
        November = reader.GetInt32("November");
        December = reader.GetInt32("December");
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
