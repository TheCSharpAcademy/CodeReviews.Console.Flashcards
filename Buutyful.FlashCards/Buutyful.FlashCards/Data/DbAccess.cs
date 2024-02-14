using Buutyful.Coding_Tracker;
using Dapper;
using System.Data.SqlClient;


namespace Buutyful.FlashCards.Data;

public class DbAccess
{
    public void CreateDatabase()
    {
        using var connection = SqlConnectionFactory.Create();
        if (TablesExist(connection)) return;

        string createTablesSql = Constants.CreateTablesSql;

        connection.Execute(createTablesSql);
    }
 
    private bool TablesExist(SqlConnection connection)
    {        
        string checkTablesExistSql = Constants.CheckTableExists;

        int tableCount = connection.QueryFirstOrDefault<int>(checkTablesExistSql);

        return tableCount == 3;
    }

}