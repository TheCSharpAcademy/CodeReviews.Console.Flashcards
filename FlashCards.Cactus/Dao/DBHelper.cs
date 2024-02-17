using FlashCards.Cactus.DataModel;
using Microsoft.Data.SqlClient;

namespace FlashCards.Cactus.Dao;
public class DBHelper
{
    public static string DBConnectionStr = System.Configuration.ConfigurationManager.AppSettings.Get("DBConnection");

    public static void DropStackTable()
    {
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"IF OBJECT_ID('Stack') IS NOT NULL 
                                    DROP TABLE Stack;";
            command.ExecuteNonQuery();
        }
    }

    public static void CreateStackTable()
    {
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE Stack (
                    sid INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
                    name [NVARCHAR](50) UNIQUE NOT NULL
                );
            ";
            command.ExecuteNonQuery();
        }
    }

    public static void InsertSomeData()
    {
        StackDao stackDao = new StackDao(DBConnectionStr);
        stackDao.Insert(new Stack("Test1"));
        stackDao.Insert(new Stack("Test2"));
    }
}

