using System.Data;
using Dapper;
using System.Data.SqlClient;
using FlashcardsLibrary;

namespace CodingTrackerLibrary;

public static class SqlExecutionService
{
    public static bool ExecuteCommand<T>(string sql, T model)
    {
        try
        {
            using IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString);
            
            connection.Open();
            connection.Execute(sql, model);
            connection.Close();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ocurred: {ex.Message}");
            return false;
        }
    }

    public static List<T> GetListModels<T>(string sql)
    {
        List<T> result = new List<T>();

        try
        {
            using IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString);
            
            connection.Open();
            result = connection.Query<T>(sql).ToList();
            connection.Close();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ocurred: {ex.Message}");
        }

        return result;
    }

    public static List<TValue> GetListModelsByKey<TKey, TValue>(string sql, string field, TKey id)
    {
        List<TValue> result = new List<TValue>();

        try
        {
            using IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString);

            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add($"@{field}", id);
            result = connection.Query<TValue>(sql, parameters).ToList();
            connection.Close();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ocurred: {ex.Message}");
        }

        return result;
    }

    public static TValue GetModelByKey<TKey, TValue>(string sql, string field, TKey id) where TValue : class
    {
        TValue result = null;

        try
        {
            using IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString);
            
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add($"@{field}", id);
            result = connection.QueryFirstOrDefault<TValue>(sql, parameters);
            connection.Close();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ocurred: {ex.Message}");
        }

        return result;
    }
}