using Microsoft.Data.SqlClient;
namespace Program;
class Program
{
    public static void Main (string[] args)
    {
//Menu
    DBController.CreateTables(DBController.ConnectDB());
    UserInput.MainMenu();
    }
}


