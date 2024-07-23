using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Flashcards.Data.Entities;
using Flashcards.Data.Exceptions;

namespace Flashcards.Data.Managers;

/// <summary>
/// Partial class for StudySession entity specific data manager methods against an T-SQL database.
/// </summary>
public partial class SqlDataManager
{
    #region Methods

    public void AddStudySession(int stackId, DateTime dateTime, int score)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();
        
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.AddStudySession";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@StackId", SqlDbType.Int).Value = stackId;
        command.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = dateTime;
        command.Parameters.Add("@Score", SqlDbType.Int).Value = score;
        command.ExecuteNonQuery();
    }

    public IReadOnlyList<StudySessionEntity> GetStudySessions()
    {
        var output = new List<StudySessionEntity>();

        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetStudySessions";
        command.CommandType = CommandType.StoredProcedure;
        
        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new StudySessionEntity(reader));
        }

        return output;
    }

    #endregion
}
