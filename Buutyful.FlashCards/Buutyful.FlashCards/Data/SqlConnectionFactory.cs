using System.Configuration;
using System.Data.SqlClient;
namespace Buutyful.FlashCards.Data;

public static class SqlConnectionFactory
{
    private static readonly string _connectionString =
        ConfigurationManager.ConnectionStrings["LocalDbConnection"].ConnectionString ??
        throw new Exception("Connection string is missing");
    public static SqlConnection Create() => new(_connectionString);
}
