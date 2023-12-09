using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Flashcards;

class DBController
{
    private static readonly string DBName = "FlashCardsProgram";
    private static readonly string stacksTableName = "stacks";
    private static readonly string flashcardsTableName = "flashcards";
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["FlashCardsConnectionString"].ConnectionString;


    public static void CreateDB()
    {
        using var connection = new SqlConnection(connectionString);
        
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"IF NOT EXISTS (
        SELECT name
        FROM sys.databases
        WHERE name = N'{DBName}'
        )
        CREATE DATABASE {DBName}";

        command.ExecuteNonQuery();
        connection.Close();
    }

    public static void CreateTables()
    {
        using var connection = new SqlConnection(connectionString);
             
        connection.Open();
        var tableCmd = connection.CreateCommand();
        
        tableCmd.CommandText = 
        $@"USE {DBName}
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{stacksTableName}' and xtype='U')
        CREATE TABLE dbo.{stacksTableName} (
        stackid INT IDENTITY(1,1) PRIMARY KEY,
        stackname VARCHAR(50) UNIQUE NOT NULL)";
        
        tableCmd.ExecuteNonQuery();

        tableCmd.CommandText = 
        $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{flashcardsTableName}' and xtype='U')
        CREATE TABLE dbo.{flashcardsTableName} (
        cardid INT IDENTITY(1,1) PRIMARY KEY,
        stackid INT FOREIGN KEY REFERENCES stacks(stackid) 
        ON DELETE CASCADE ON UPDATE CASCADE,
        question VARCHAR(50) NOT NULL,
        answer VARCHAR(50) NOT NULL )";
        
        tableCmd.ExecuteNonQuery();
        connection.Close();   
    }

    public static bool TableIsNotEmpty(string table)
    {
        bool tableIsNotEmpty;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        IF EXISTS(SELECT 1 FROM dbo.{table})
        SELECT 1
        ELSE SELECT 0";

        SqlDataReader reader = command.ExecuteReader();
        reader.Read();
        tableIsNotEmpty = Convert.ToBoolean(reader.GetInt32(0));

        connection.Close();
        return tableIsNotEmpty;
    }

    public static int InsertNewStack(Stacks newStack)
    {
        int insertSuccess;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        IF NOT EXISTS(SELECT * 
        FROM {stacksTableName}
        WHERE stackname = @stackName)
        INSERT INTO {stacksTableName}
        (stackname)
        VALUES
        (@stackName)";

        command.Parameters.Add("@stackName", System.Data.SqlDbType.VarChar,50).Value = newStack.StackName;
        insertSuccess = command.ExecuteNonQuery();

        connection.Close();
        return insertSuccess;   
    }

    public static List<Stacks> SelectStacks()
    {
        List<Stacks> stacksQuery = [];
 
        using var connection = new SqlConnection(connectionString);
        
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        SELECT stackid, stackname
        FROM {stacksTableName}";

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read())
        {
            stacksQuery.Add(new Stacks(reader.GetString(1), reader.GetInt32(0)));
        }

        return stacksQuery;
    }
}
// tableCmd.Parameters.Add("@table",System.Data.SqlDbType.VarChar,25).Value = table;
