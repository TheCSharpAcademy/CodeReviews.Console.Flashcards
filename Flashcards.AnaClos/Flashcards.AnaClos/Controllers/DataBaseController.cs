using Dapper;
using Microsoft.Data.SqlClient;

namespace Flashcards.AnaClos.Controllers;

public class DataBaseController
{   
    private string _connectionString;

    public DataBaseController(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Execute(string commandText)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            //connection.Open();
            connection.Execute(commandText);
            //connection.Close();
        }
    }

    public int Execute<T>(string sql,T genericObject)
    {
        int rowsAffected = 0;
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                rowsAffected = connection.Execute(sql, genericObject);

            }
            catch (SqlException)
            {
                throw;
            }            
        }
        return rowsAffected;
    }
    public T QuerySingle<T>(string sql)
    {
        T value = default(T);
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                value = connection.QuerySingle<T>(sql);
            }
            catch (SqlException)
            {
                throw;
            }
        }
        return value;
    }
    public T QuerySingle<T>(string sql, string text)
    {
        T newObject;
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                newObject = connection.QuerySingle<T>(sql, text);
            }
            catch (SqlException)
            {
                throw;
            }
        }
        return newObject;
    }

    public void QueryPrueba()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {

                var sql = @"INSERT INTO Stacks (Name) VALUES (@Name);
                SELECT @@ROWCOUNT AS RowsInserted, CAST(SCOPE_IDENTITY() AS INT) AS NewID;";

                var result = connection.QuerySingle<(int RowsInserted, int NewID)>(sql, new { Name = "prueba" }, transaction);

                transaction.Commit();

                Console.WriteLine("Rows Inserted: " + result.RowsInserted);
                Console.WriteLine("New ID: " + result.NewID);
            }
        }
    }

    public T QuerySingle<T>(string sql, int value)
    {
        T newObject;
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                newObject = connection.QuerySingle<T>(sql, value);
            }
            catch (SqlException)
            {
                throw;
            }
        }
        return newObject;
    }


    public List<T> Query<T>(string sql, T genericObject)
    {
        List<T> list = new List<T>();
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                list = connection.Query<T>(sql, genericObject).ToList();
            }
            catch (SqlException)
            {
                throw;
            }
        }
        return list;
    }

    public List<T> Query<T>(string sql)
    {
        List<T> list = new List<T>();
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                list = connection.Query<T>(sql).ToList();
            }
            catch (SqlException)
            {
                throw;
            }
        }
        return list;
    }
}