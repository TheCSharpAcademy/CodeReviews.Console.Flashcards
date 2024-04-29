using Dapper;
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
        conn.Execute(@"CREATE TABLE StudySessions (StudySessionId INT PRIMARY KEY IDENTITY(3000,1), Fk_StackID INT NOT NULL FOREIGN KEY REFERENCES Stacks(StackID) ON DELETE CASCADE, SessionDate DATE NOT NULL, SessionScore INT NOT NULL)");
    }

}

