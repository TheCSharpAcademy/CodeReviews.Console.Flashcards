using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Flashcards;

class DBController
{
    public static readonly string DBName = "FlashCardsProgram";
    public static readonly string stacksTableName = "stacks";
    public static readonly string flashcardsTableName = "flashcards";
    public static readonly string studysessionsTableName = "studysessions";
    private static string? connectionString;

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
        stackid INT IDENTITY(1,1) UNIQUE NOT NULL,
        stackname VARCHAR(50) PRIMARY KEY)";
        
        tableCmd.ExecuteNonQuery();

        tableCmd.CommandText = 
        $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{flashcardsTableName}' and xtype='U')
        CREATE TABLE dbo.{flashcardsTableName} (
        cardid INT IDENTITY(1,1) PRIMARY KEY,
        stackname VARCHAR(50) FOREIGN KEY REFERENCES stacks(stackname) ON DELETE CASCADE ON UPDATE CASCADE,
        question NVARCHAR(600) NOT NULL,
        answer NVARCHAR(600) NOT NULL, )";
        
        tableCmd.ExecuteNonQuery();

        tableCmd.CommandText = 
        $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{studysessionsTableName}' and xtype='U')
        CREATE TABLE dbo.{studysessionsTableName} (
        studysessionid INT IDENTITY(1,1) PRIMARY KEY,
        stackname VARCHAR(50) FOREIGN KEY REFERENCES stacks(stackname) ON DELETE CASCADE ON UPDATE CASCADE,
        studysessiondate DATETIME,
        score FLOAT)";

        tableCmd.ExecuteNonQuery();

        connection.Close();   
    }

    public static string? DBInit()
    {
        string? errorMessage = null;
        bool insertData = false;

        try
        {
            connectionString = ConfigurationManager.ConnectionStrings["FlashCardsConnectionString"].ConnectionString;
            CreateDB();
            CreateTables();

            insertData = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("PopulateDB"));
        }
        catch
        {
            errorMessage += "Please configure your connection string before using the program. ";
        }

        if(insertData && errorMessage == null)
        {
            try
            {
                Helpers.DBPopulate();
            }
            catch
            {
                errorMessage += "Please check your csv files. ";
            }
        }
        return errorMessage;
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

    public static bool StackHasCards(Stacks selectedStack)
    {
        bool tableIsNotEmpty;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        IF EXISTS(SELECT 1 FROM dbo.{flashcardsTableName} WHERE stackname = @stackName)
        SELECT 1
        ELSE SELECT 0";

        command.Parameters.Add("@stackname", System.Data.SqlDbType.VarChar, 50).Value = selectedStack.StackName;
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

    public static void DeleteStack(Stacks? selectedStack) //To improve
    {

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        DELETE FROM {stacksTableName}
        WHERE stackname = @stackName";

        command.Parameters.Add("@stackName", System.Data.SqlDbType.VarChar,50).Value = selectedStack?.StackName;
        command.ExecuteNonQuery();

        connection.Close();
    }

    public static int ModifyStack(Stacks stackToModify, Stacks newStack)
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
        command.Parameters.Add("@stackName", System.Data.SqlDbType.VarChar,50).Value = stackToModify.StackName;
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
        SELECT cardid, stackname, question, answer
        FROM {flashcardsTableName}
        WHERE stackname = @stackName
        ORDER BY question ASC";

        command.Parameters.Add("@stackName", System.Data.SqlDbType.VarChar, 50).Value = selectedStack?.StackName;

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read())
        {
            flashcardsQuery.Add(new Cards(reader.GetString(1), reader.GetString(2),
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
        (stackname, question, answer)
        VALUES
        (@stackName, @question, @answer)";

        command.Parameters.Add("@stackName", System.Data.SqlDbType.VarChar, 50).Value = newCard.StackName;
        command.Parameters.Add("@question", System.Data.SqlDbType.NVarChar,600).Value = newCard.Question;
        command.Parameters.Add("@answer", System.Data.SqlDbType.NVarChar,600).Value = newCard.Answer;

        insertSuccess = command.ExecuteNonQuery();

        connection.Close();
        return insertSuccess;   
    }

    public static int ModifyCard(Cards cardToModify)
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

        command.Parameters.Add("@question", System.Data.SqlDbType.NVarChar, 600).Value = cardToModify.Question;
        command.Parameters.Add("@answer", System.Data.SqlDbType.NVarChar,600).Value = cardToModify.Answer;
        command.Parameters.Add("@cardid", System.Data.SqlDbType.Int).Value = cardToModify.CardID;

        insertSuccess = command.ExecuteNonQuery();

        connection.Close();
        return insertSuccess;   
    }

    public static void DeleteCard(Cards card)
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
        WHERE stackname = @stackName
        ORDER BY NEWID()";

        command.Parameters.Add("@stackName", System.Data.SqlDbType.VarChar, 50).Value = stack?.StackName;

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read())
        {
            flashcardsQuery.Add(new Cards(reader.GetString(1), reader.GetString(2),
            reader.GetString(3),reader.GetInt32(0)));
        }
        connection.Close();
        return flashcardsQuery;
    }

    public static int InsertNewStudySession(StudySession newStudySession)
    {
        int insertSuccess;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        INSERT INTO {studysessionsTableName}
        (stackname, studysessiondate, score)
        VALUES
        (@stackname, @studysessiondate, @score)";

        command.Parameters.Add("@stackname", System.Data.SqlDbType.VarChar, 50).Value = newStudySession.StackName;
        command.Parameters.Add("@studysessiondate", System.Data.SqlDbType.DateTime).Value = newStudySession.Date;
        command.Parameters.Add("@score", System.Data.SqlDbType.Float).Value = newStudySession.Score;

        insertSuccess = command.ExecuteNonQuery();

        connection.Close();
        return insertSuccess;   
    }

    public static List<StudySession> SelectStudySessions()
    {
        List<StudySession> studySessionsQuery = [];
 
        using var connection = new SqlConnection(connectionString);
        
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        SELECT studysessionid, stackname, studysessiondate, score
        FROM {studysessionsTableName}
        ORDER BY studysessiondate ASC";

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read())
        {
            studySessionsQuery.Add(new StudySession(reader.GetString(1), reader.GetDateTime(2),
            reader.GetDouble(3),reader.GetInt32(0)));
        }

        connection.Close();
        return studySessionsQuery;
    }

    public static List<List<object>> GetStudySessionsReports()
    {
        List<List<object>> studySessionReport = [];
    
        using var connection = new SqlConnection(connectionString);

        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
        $@"USE {DBName}
        SELECT YEAR(studysessiondate) AS 'year', MONTH(studysessiondate) AS 'month', stackname,
        COUNT(studysessionid) AS 'studysessionqty',  AVG(score)*100 AS 'avgscore'
        FROM {studysessionsTableName}
        GROUP BY stackName, YEAR(studysessiondate), MONTH(studysessiondate)
        ORDER BY 'year', 'month', stackname";

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read())
        {
            studySessionReport.Add([reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), 
                reader.GetInt32(3), reader.GetDouble(4).ToString("0.#")]);
        }

        connection.Close();
        return studySessionReport;
    }

}
