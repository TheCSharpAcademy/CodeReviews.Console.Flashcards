using FlashCards.Cactus.DataModel;
using Microsoft.Data.SqlClient;

namespace FlashCards.Cactus.Dao;
public class DBHelper
{
    public static string DBConnectionStr = System.Configuration.ConfigurationManager.AppSettings.Get("DBConnection");

    public static void DropTables()
    {
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"IF OBJECT_ID('FlashCard') IS NOT NULL 
                                    DROP TABLE FlashCard;
                                    IF OBJECT_ID('Stack') IS NOT NULL 
                                    DROP TABLE Stack;
                                    ";
            command.ExecuteNonQuery();
        }
    }

    public static void CreateTables()
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
                CREATE TABLE FlashCard
                (
                    fid INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
                    front VARCHAR(200),
                    back VARCHAR(200),
                    sid INT FOREIGN KEY REFERENCES Stack(sid)
                );
            ";
            command.ExecuteNonQuery();
        }
    }

    public static void InsertSomeData()
    {
        StackDao stackDao = new StackDao(DBConnectionStr);
        stackDao.Insert(new Stack("Words"));
        stackDao.Insert(new Stack("Algorithm"));

        FlashCardDao cardDao = new FlashCardDao(DBConnectionStr);
        cardDao.Insert(new FlashCard(1, 1, "Freedom", "ziyou"));
        cardDao.Insert(new FlashCard(2, 2, "1*2=", "3"));
    }
}

