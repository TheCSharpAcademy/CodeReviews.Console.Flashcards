using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Flash_Cards.Lawang.Controller;

public class StackController
{
    private readonly string _connectionString;
    public StackController(string cs)
    {
       _connectionString = cs; 
    }

    public void CreateStackTable()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string createSQL = 
                @"IF NOT EXISTS (
                    SELECT * FROM sys.tables
                    WHERE name = 'stacks' AND schema_id = SCHEMA_ID('dbo') 
                    )
                    BEGIN
                        CREATE TABLE stacks
                        (Id INT PRIMARY KEY IDENTITY(1,1),
                        Name VARCHAR(30) NOT NULL
                        );
                    END";

            connection.Execute(createSQL);

        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public int CreateStack(string stack)
    {
        int rowsAffected = 0;
        try
        {
            string insertSQL =
                @"INSERT INTO stacks
                (Name)
                VALUES(@name);";
            
            var param = new {@name = stack};
            using var connection = new SqlConnection(_connectionString);

            return connection.Execute(insertSQL, param);
        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return rowsAffected;
    }

    public void SeedValueForTesting()
    {
        try
        {
            string insertSQL = 
                @"
                IF NOT EXISTS(SELECT 1 FROM stacks)
                BEGIN
                    INSERT INTO stacks
                    (Name)
                    VALUES ('Spanish'),
                           ('French'),
                           ('Italian')
                END";
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(insertSQL);
        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public List<Stack> GetAllStacks()
    {
        try
        {
            string getAllSQL = 
                @"SELECT * FROM stacks";
            
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<Stack>(getAllSQL).ToList();
        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return new List<Stack>();
    }

    public int UpdateStack(Stack stack)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string updateSQL =
                @"UPDATE stacks
                SET Name = @name
                WHERE Id = @id";
            
            var param = new {@name = stack.Name, @id = stack.Id};
            return connection.Execute(updateSQL, param);

        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return 0;
    }

    public int DeleteStack(int Id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string deleteSQL = 
                @"DELETE FROM stacks
                WHERE Id = @id";

            var param = new {@id = Id};

            return connection.Execute(deleteSQL, param);

        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return 0;
    }
}
