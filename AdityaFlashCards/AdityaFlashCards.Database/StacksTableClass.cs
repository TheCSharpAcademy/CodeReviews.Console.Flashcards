using Dapper;
using System.Data.SqlClient;

namespace AdityaFlashCards.Database;

internal class StacksTableClass
{
    private string? _connectionString;

    public StacksTableClass(string? connectionString)
    {
        _connectionString = connectionString;
    }

    internal void CreateTable()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute(@"CREATE TABLE Stacks (StackID INTEGER PRIMARY KEY IDENTITY, Name Varchar(100) UNIQUE NOT NULL)");
    }
}

