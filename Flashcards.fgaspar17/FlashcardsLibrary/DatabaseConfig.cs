using System.Data;
using System.Data.SqlClient;
using Dapper;


namespace FlashcardsLibrary;

public class DatabaseConfig
{
    public void InitializeDatabase(string databaseName)
    {
        try
        {
            using IDbConnection connection = new SqlConnection(GlobalConfig.SetupConnectionString);

            connection.Open();
            string? db = connection.QuerySingleOrDefault<string>(@$"SELECT name FROM sys.databases WHERE name = '{databaseName}'");

            if (db == null)
            {
                connection.Execute($"CREATE DATABASE {databaseName};");
                connection.ChangeDatabase(databaseName);

                using IDbTransaction transaction = connection.BeginTransaction();
                string scriptSql = File.ReadAllText("DatabaseScript.txt");
                connection.Execute(scriptSql, transaction: transaction);
                transaction.Commit();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error ocurred creating the database structure. Error {e.Message}");
        }
    }
}