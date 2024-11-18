using System.Configuration;
using Microsoft.Data.SqlClient;

namespace FlashCards.Data;

internal abstract class DataBaseController<T>
{
    protected void InitDataBase()
    {
        using (var connection = CreateConnection()) // create the database if it doesn't exist
        {
            connection.Open();

            using (var cmd = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FlashCards') CREATE DATABASE [FlashCards];", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        InitStackTable();
        InitCardTable();
        InitStudyTable();
    }

    private static void InitStackTable()
    {
        using var connection = CreateConnection();

        connection.Open();
        var checkTableCommand = connection.CreateCommand();
        checkTableCommand.CommandText = @"
                SELECT TABLE_NAME 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'stacks';
            ";

        var tableName = checkTableCommand.ExecuteScalar() as string;

        if (string.IsNullOrEmpty(tableName))
        {
            var createQuery = @"
                    CREATE TABLE stacks 
                    (
	                    id INT IDENTITY(1,1) PRIMARY KEY,
	                    name NVARCHAR(25) UNIQUE NOT NULL
                    );
                ";

            using var createCommand = connection.CreateCommand();
            createCommand.CommandText = createQuery;
            createCommand.ExecuteNonQuery();
        }
        connection.Close();
    }
    private static void InitCardTable()
    {
        using var connection = CreateConnection();

        connection.Open();
        var checkTableCommand = connection.CreateCommand();
        checkTableCommand.CommandText = @"
                SELECT TABLE_NAME 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'cards';
            ";

        var tableName = checkTableCommand.ExecuteScalar() as string;

        if (string.IsNullOrEmpty(tableName))
        {
            var createQuery = @"
                    CREATE TABLE cards 
                    (
	                    FK_stack_id int,
	                    cardnumber int identity(1,1) PRIMARY KEY,
	                    front varchar(max),
	                    back varchar(max),
	                    FOREIGN KEY (FK_stack_id) REFERENCES stacks(id) ON DELETE CASCADE,
                    );
                ";

            using var createCommand = connection.CreateCommand();
            createCommand.CommandText = createQuery;
            createCommand.ExecuteNonQuery();
        }
        connection.Close();
    }
    private static void InitStudyTable()
    {
        using var connection = CreateConnection();

        connection.Open();

        var checkTableCommand = connection.CreateCommand();
        checkTableCommand.CommandText = @"
                SELECT TABLE_NAME 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'study';
            ";

        var tableName = checkTableCommand.ExecuteScalar() as string;

        if (string.IsNullOrEmpty(tableName))
        {
            var createQuery = @"
                    CREATE TABLE study 
                    (
	                    FK_stack_id INT,
	                    session_date DATE,
	                    score INT NOT NULL,
	                    session_id INT IDENTITY(1,1) primary key,
	                    FOREIGN KEY (FK_stack_id) REFERENCES stacks(id) ON DELETE CASCADE
                    );
                ";

            using var createCommand = connection.CreateCommand();
            createCommand.CommandText = createQuery;
            createCommand.ExecuteNonQuery();
        }
        connection.Close();
    }
    private static string LoadConnectionString(string id = "Default")
    {
        return ConfigurationManager.ConnectionStrings[id].ConnectionString;
    }
    protected static SqlConnection CreateConnection()
    {
        return new SqlConnection(LoadConnectionString());
    }
    public abstract List<T> ReadAllRows();

    public abstract void InsertRow(T classObject);
    public abstract void DeleteRow(int _id);
    public abstract void UpdateRow(T classObject);

}

