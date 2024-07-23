using System.Data;
using System.Data.SqlClient;
using Flashcards.Data.Entities;

namespace Flashcards.Data.Managers;

/// <summary>
/// Partial class for StudySessionReport entity specific data manager methods against an T-SQL database.
/// </summary>
public partial class SqlDataManager
{
    #region Methods

    public IReadOnlyList<StudySessionReportEntity> GetAverageStudySessionScoreReportByYear(DateTime dateTime)
    {
        var output = new List<StudySessionReportEntity>();

        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetAverageStudySessionScoreReportByYear";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = dateTime.ToString("yyyy");

        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new StudySessionReportEntity(reader));
        }

        return output;
    }

    public IReadOnlyList<StudySessionReportEntity> GetTotalStudySessionsReportByYear(DateTime dateTime)
    {
        var output = new List<StudySessionReportEntity>();

        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetTotalStudySessionsReportByYear";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = dateTime.ToString("yyyy");

        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new StudySessionReportEntity(reader));
        }

        return output;
    }

    #endregion
}
