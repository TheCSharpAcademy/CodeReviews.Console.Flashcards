using System.Data.SqlClient;

public class DbOperations : DbBase
{
    public void InitDb()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();
    
            tableCommand.CommandText = SqlCommands["createStackTable"];

            tableCommand.ExecuteNonQuery();

            tableCommand.CommandText = SqlCommands["createFlashcardsTable"];
    
            tableCommand.ExecuteNonQuery();
    
            connection.Close();
        }
    }
}