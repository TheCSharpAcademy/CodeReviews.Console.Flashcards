using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
internal class DatabaseManager
{
    internal DatabaseManager()
    {
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("dbConnection")))
        {
            connection.Open();

            String sql = "SELECT stacks FROM stacks";

            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("{0}", reader.GetString(0));
            }

            connection.Close();
        }
    }
}