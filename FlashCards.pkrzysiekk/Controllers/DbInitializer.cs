using System.Configuration;
using Microsoft.Data.SqlClient;
using FlashCards.Models;

namespace FlashCards.Controllers
{
    public class DbInitializer : DbController
    {
        private string? serverConnectionString;
        public DbInitializer()
        {
            serverConnectionString = ConfigurationManager.ConnectionStrings["ServerConnectionString"].ConnectionString;
            InitializeDB();

        }
        private void InitializeDB()
        {
            if (serverConnectionString == null)
            {
                throw new Exception("No connection string!");
            }

            using (SqlConnection connection = new SqlConnection(serverConnectionString))
            { 
                connection.Open();
                try
                {
                    string? createDbQuery = ConfigurationManager.AppSettings["createDB"];
                    string? createDbTables = ConfigurationManager.AppSettings["createTables"]; 
                    if(createDbQuery == null || createDbTables == null)
                    {
                        throw new Exception("No configuration query");
                    }

                    using (SqlCommand command = new SqlCommand(createDbQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(createDbTables, connection))
                    {
                        command.ExecuteNonQuery();
                    } 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

    }
}
