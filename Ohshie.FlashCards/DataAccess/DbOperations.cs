using System.Data.SqlClient;
using Ohshie.FlashCards.DataAccess;

public class DbOperations : DbBase
{
    public void InitDb()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();
            
            tableCommand.CommandText = SqlCommands["createDeckTable"]
                                       +SqlCommands["createFlashcardsTable"]
                                       +SqlCommands["createStudySessionTable"];

            tableCommand.ExecuteNonQuery();
            
    
            connection.Close();
        }
    }
}