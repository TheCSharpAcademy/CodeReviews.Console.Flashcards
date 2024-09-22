using System.Data.SqlClient;
namespace Flashcards_JvR_Hannes
{
    internal class Program
    {
        public static string ConnectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flashcardsdb.mdf")};Integrated Security=True;";
        static void Main(string[] args)
        {
            ConfigureBeforeLaunching.EnsureDatabaseExists();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                ConfigureBeforeLaunching.CreateStacksTable(connection);
                ConfigureBeforeLaunching.CreateFlashcardsTable(connection);
                ConfigureBeforeLaunching.CreateStudySession(connection);

                ConfigureBeforeLaunching.InsertInitialData(connection);
            }

            SqlController.DatabaseManager.InitializeDatabase();

            SqlController.MenuDisplay();
        }
    }
}
