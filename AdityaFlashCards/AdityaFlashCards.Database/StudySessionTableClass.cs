using System.Data.SqlClient;

namespace AdityaFlashCards.Database;

internal class StudySessionTableClass
{
    private string? _connectionString;

    public StudySessionTableClass(string? connectionString)
    {
        _connectionString = connectionString;
    }

    internal void CreateTable()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
    }
}

