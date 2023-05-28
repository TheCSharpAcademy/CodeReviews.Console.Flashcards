using System.Data.SqlClient;

namespace Flashcards.Models;

internal class StudyDTO
{
    public string Stack { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.MinValue;
    public double Score { get; set; }

    public StudyDTO() { }

    public StudyDTO(SqlDataReader reader)
    {
        Stack = reader.GetString(0);
        Date = reader.GetDateTime(1);
        Score = reader.GetDouble(2);
    }
}
