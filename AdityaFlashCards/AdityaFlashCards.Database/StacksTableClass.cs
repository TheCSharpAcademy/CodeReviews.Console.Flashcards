using Dapper;
using System.Data.SqlClient;
using System.Diagnostics;

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
        conn.Execute(@"CREATE TABLE Stacks (StackID INTEGER PRIMARY KEY IDENTITY(1000,1), Name Varchar(100) UNIQUE NOT NULL)");
    }

    internal void InsertNewStack(string stackName) {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("INSERT INTO Stacks (Name) VALUES (@StackName)", new { StackName = stackName});
    }

    internal void DeleteStack(int stackID)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("DELETE FROM Stacks WHERE StackID = @stackID", new { stackID});
    }

}

