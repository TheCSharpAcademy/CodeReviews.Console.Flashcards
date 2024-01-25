using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataAccess;

public class SqlDataAccessMethods : IDataAccess
{
    private string? initConnectionString;
    private string? dbConnString;
    private string? dbCreationScript;
    private string? tableCreationScript;
    private string? sampleData;

    public SqlDataAccessMethods()
    {
        initConnectionString = GetConnectionStringFromSettings("MasterConnString");
        dbConnString = GetConnectionStringFromSettings("FlashcardsConnString");
        dbCreationScript = File.ReadAllText(@"..\DataAccess\Databaseinit.txt");
        tableCreationScript = File.ReadAllText(@"..\DataAccess\CreateTables.txt");
        sampleData = File.ReadAllText(@"..\DataAccess\SampleData.txt");
    }

    public void InitDatabase()
    {
        using (SqlConnection connection = new SqlConnection(initConnectionString))
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = dbCreationScript;
            command.ExecuteNonQuery();
            connection.Close();
        }

        using(SqlConnection connection1 = new SqlConnection(dbConnString))
        {
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandText = tableCreationScript;
            command1.ExecuteNonQuery();
            
            command1 = connection1.CreateCommand();
            command1.CommandText = sampleData;
            command1.ExecuteNonQuery();
            connection1.Close();
        }
    }
    private string? GetConnectionStringFromSettings(string desiredString)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
        IConfiguration configuration = builder.Build();
        return configuration.GetConnectionString(desiredString);
    }
    public void InsertCard()
    {
        throw new NotImplementedException();
    }
    public void DeleteCard()
    {
        throw new NotImplementedException();
    }
    public void InsertStack()
    {
        throw new NotImplementedException();
    }
    public void DeleteStack()
    {
        throw new NotImplementedException();
    }
}
