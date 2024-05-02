using Dapper;
using System.Data.SqlClient;
using System.Diagnostics;
using AdityaFlashCards.Database.Models;
using System.Collections.Generic;

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

    internal void DeleteStack(string stackName)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("DELETE FROM Stacks WHERE Name = @stackName", new { stackName });
    }

    internal int GetStackIdFromStackName(string stackName)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        string sql = "SELECT StackID FROM Stacks WHERE Name = @stackName";
        int result = conn.QueryFirstOrDefault<int>(sql, new { stackName });
        return result;
    }


    internal List<Stack> GetAllStacks()
    {
        List<Stack> records = new List<Stack> ();
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        string sql = "SELECT * FROM Stacks ORDER BY StackID";
        var result = conn.Query<Stack>(sql);
        records.AddRange(result);
        return records;
    }

}

