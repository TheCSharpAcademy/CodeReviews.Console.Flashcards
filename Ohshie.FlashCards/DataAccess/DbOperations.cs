using System.Data.SqlClient;
using Ohshie.FlashCards.CardsManager;
using Ohshie.FlashCards.DataAccess;
using Ohshie.FlashCards.StacksManager;

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