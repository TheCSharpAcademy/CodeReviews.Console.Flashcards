using System.Data.SqlClient;

namespace AdityaFlashCards.Database;

internal class FlashCardsTableClass
{
    private string? _connectionString;

    public FlashCardsTableClass(string? connectionString)
    {
        _connectionString = connectionString;
    }

    internal void CreateTable()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
    }
}

