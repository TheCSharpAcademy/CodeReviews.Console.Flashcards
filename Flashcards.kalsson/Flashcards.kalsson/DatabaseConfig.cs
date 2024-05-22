using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Flashcards.kalsson;

public class DatabaseConfig
{
    private readonly IConfiguration _configuration;
    
    public DatabaseConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Creates and returns a new SQL connection using the connection string.
    /// </summary>
    public IDbConnection NewConnection
    {
        get
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}