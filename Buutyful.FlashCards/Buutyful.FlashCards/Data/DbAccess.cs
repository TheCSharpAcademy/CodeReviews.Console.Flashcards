using Buutyful.Coding_Tracker;
using Dapper;
using System.Data.SqlClient;


namespace Buutyful.FlashCards.Data;

public static class DbAccess
{
    public static void CreateDatabase()
    {
        using var connection = SqlConnectionFactory.Create();
        if (TablesExist(connection)) return;

        string createTablesSql = Constants.CreateTablesSql;

        connection.Execute(createTablesSql);
        SeedDecks(connection);
        SeedFlashCards(connection);
    }
 
    private static bool TablesExist(SqlConnection connection)
    {        
        string checkTablesExistSql = Constants.CheckTableExists;

        int tableCount = connection.QueryFirstOrDefault<int>(checkTablesExistSql);

        return tableCount == 3;
    }
    private static void SeedDecks(SqlConnection connection)
    {
        var seedDecksSql = Constants.SeedDeckSql;

        connection.Execute(seedDecksSql);
    }
    private static void SeedFlashCards(SqlConnection connection)
    {
        var seedFlashCardsSql = Constants.SeedFlashCards;

        connection.Execute(seedFlashCardsSql);
    }
}