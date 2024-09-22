
using System;
using System.Data;

namespace FlashcardApp.Core.Data;

public class DatabaseContext
{
     private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}