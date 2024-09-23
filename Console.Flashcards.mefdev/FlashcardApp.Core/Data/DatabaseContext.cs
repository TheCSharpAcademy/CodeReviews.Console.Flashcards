
using Microsoft.Data.SqlClient;

namespace FlashcardApp.Core.Data;

public class DatabaseContext
{
     private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}