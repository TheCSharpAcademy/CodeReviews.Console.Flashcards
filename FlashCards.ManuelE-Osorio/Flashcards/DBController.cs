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
        question VARCHAR(50) UNIQUE NOT NULL,
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
        FROM {stacksTableName}
        ORDER BY stackname ASC";

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read())
        {
            stacksQuery.Add(new Stacks(reader.GetString(1), reader.GetInt32(0)));
        }
        connection.Close();
        return stacksQuery;
    }

    public static void DeleteStack(string stackName) //To improve
    {

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        DELETE FROM {stacksTableName}
        WHERE stackname = @stackName";

        command.Parameters.Add("@stackName", System.Data.SqlDbType.VarChar,50).Value = stackName;
        command.ExecuteNonQuery();

        connection.Close();
    }

    public static int ModifyStack(Stacks modifyStack, Stacks newStack)
    {
        int insertSuccess;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        UPDATE {stacksTableName}
        SET
            stackname = @newStackName
        WHERE stackname = @stackName";

        command.Parameters.Add("@newStackName", System.Data.SqlDbType.VarChar,50).Value = newStack.StackName;
        command.Parameters.Add("@stackName", System.Data.SqlDbType.VarChar,50).Value = modifyStack.StackName;
        insertSuccess = command.ExecuteNonQuery();

        connection.Close();
        return insertSuccess; 
    }

    public static List<Cards> SelectFlashcards(Stacks? selectedStack)
    {
        List<Cards> flashcardsQuery = [];
 
        using var connection = new SqlConnection(connectionString);
        
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        SELECT cardid, stackid, question, answer
        FROM {flashcardsTableName}
        WHERE stackid = @stackID
        ORDER BY question ASC";

        command.Parameters.Add("@stackID", System.Data.SqlDbType.Int).Value = selectedStack?.StackID;

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read())
        {
            flashcardsQuery.Add(new Cards(reader.GetInt32(1), reader.GetString(2),
            reader.GetString(3),reader.GetInt32(0)));
        }

        connection.Close();
        return flashcardsQuery;
    }

    public static int InsertNewCard(Cards newCard)
    {
        int insertSuccess;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        INSERT INTO {flashcardsTableName}
        (stackid, question, answer)
        VALUES
        (@stackid, @question, @answer)";

        command.Parameters.Add("@stackid", System.Data.SqlDbType.Int).Value = newCard.StackID;
        command.Parameters.Add("@question", System.Data.SqlDbType.VarChar,50).Value = newCard.Question;
        command.Parameters.Add("@answer", System.Data.SqlDbType.VarChar,50).Value = newCard.Answer;

        insertSuccess = command.ExecuteNonQuery();

        connection.Close();
        return insertSuccess;   
    }

    public static int ModifyCard(Cards modifyCard)
    {
        int insertSuccess;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        UPDATE {flashcardsTableName}
        SET
        question = @question,
        answer = @answer
        WHERE cardid = @cardid";

        command.Parameters.Add("@question", System.Data.SqlDbType.VarChar, 50).Value = modifyCard.Question;
        command.Parameters.Add("@answer", System.Data.SqlDbType.VarChar,50).Value = modifyCard.Answer;
        command.Parameters.Add("@cardid", System.Data.SqlDbType.Int).Value = modifyCard.CardID;

        insertSuccess = command.ExecuteNonQuery();

        connection.Close();
        return insertSuccess;   
    }

    public static void DeleteCard(Cards card) //To improve
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        DELETE FROM {flashcardsTableName}
        WHERE cardid = @cardID";

        command.Parameters.Add("@cardID", System.Data.SqlDbType.Int).Value = card.CardID;
        command.ExecuteNonQuery();

        connection.Close();
    }    

    public static List<Cards> SelectCardsStudySession(int quantity, Stacks? stack)
    {
        List<Cards> flashcardsQuery = [];
 
        using var connection = new SqlConnection(connectionString);
        
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        SELECT TOP {quantity} * FROM {flashcardsTableName} 
        WHERE stackid = @stackID
        ORDER BY NEWID()";

        command.Parameters.Add("@stackID", System.Data.SqlDbType.Int).Value = stack?.StackID;

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read())
        {
            flashcardsQuery.Add(new Cards(reader.GetInt32(1), reader.GetString(2),
            reader.GetString(3),reader.GetInt32(0)));
        }
        connection.Close();
        return flashcardsQuery;
    }
}
