using System.Configuration;
using System.Data.SqlClient;

namespace FlashcardsLibrary;
public class CrudController
{
    private readonly string name = "FlashCardsDB";
    private string ConnectionString(string connectionString)
    {
        return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
    }
}
