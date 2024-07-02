using Microsoft.Data.SqlClient;
using Flashcards.ukpagrace.Entity;
using Spectre.Console;
using System.Configuration;

namespace Flashcards.ukpagrace.Database;

class StackDatabase
{
    string connectionString = ConfigurationManager.AppSettings["ConfigurationString"] ?? string.Empty;
    public void CreateStack()
    {
        try
        {
            using SqlConnection sqlConnection = new(connectionString);
            using SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = @"
                   IF NOT EXISTS(
                        SELECT * 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'stack' AND TABLE_SCHEMA = 'dbo'
                    )
                    BEGIN
                        CREATE TABLE dbo.stack(
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            StackName VARCHAR(255) NOT NULL UNIQUE
                        );
                    END
                ";
            sqlConnection.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Insert(string stackName)
    {
        try
        {
            using SqlConnection sqlConnection = new(connectionString);
            using SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = @"INSERT INTO stack VALUES (@StackName)";
            cmd.Parameters.AddWithValue("@StackName", stackName);
            sqlConnection.Open();
            cmd.ExecuteNonQuery();
            AnsiConsole.MarkupLine("[green]Inserted 1 record into stack[/]");
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 2627)
        {
            AnsiConsole.MarkupLine("[red]StackName Exists[/]");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Update(int stackId, string stackName)
    {
        try
        {
            using SqlConnection sqlConnection = new(connectionString);
            using SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = @"UPDATE stack SET StackName=@StackName WHERE Id=@Id";
            cmd.Parameters.AddWithValue("@StackName", stackName);
            cmd.Parameters.AddWithValue("@Id", stackId);
            sqlConnection.Open();
            var affectedRecords = cmd.ExecuteNonQuery();
            Console.WriteLine($"updated {affectedRecords} record into stack");
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 2627)
        {
            AnsiConsole.Markup("[red]StackName Exists[/]");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Delete(int stackId)
    {
        try
        {
            using SqlConnection sqlConnection = new(connectionString);
            using SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = @"DELETE FROM stack WHERE Id=@Id";
            cmd.Parameters.AddWithValue("@Id", stackId);
            sqlConnection.Open();
            var affectedRecords = cmd.ExecuteNonQuery();
            Console.WriteLine($"deleted {affectedRecords} record into stack");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public bool IdExists(int id)
    {
        try
        {
            using SqlConnection sqlConnection = new(connectionString);
            using SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM stack WHERE Id=@Id ";
            cmd.Parameters.AddWithValue("@Id", id);
            sqlConnection.Open();
            SqlDataReader table = cmd.ExecuteReader();
            return table.HasRows;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public int GetStackId(string stackName)
    {
        try
        {
            using SqlConnection sqlConnection = new(connectionString);
            using SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = @"SELECT Id FROM stack WHERE StackName=@StackName";
            cmd.Parameters.AddWithValue("@StackName", stackName);
            sqlConnection.Open();
            var result = cmd.ExecuteScalar();

            if (result == null)
            {
                throw new Exception("Stack not found.");
            }
            int id = (int)result;
            return id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public List<StackEntity> Get()
    {
        try
        {
            List<StackEntity> records = new List<StackEntity>();
            using SqlConnection sqlConnection = new(connectionString);
            using SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM stack";
            sqlConnection.Open();
            SqlDataReader table = cmd.ExecuteReader();
            if (table.HasRows)
            {
                while (table.Read())
                {
                    records.Add(new StackEntity()
                    {
                        Id = table.GetInt32(0),
                        StackName = table.GetString(1),
                    });
                }
            }
            return records;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}