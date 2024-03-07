using System.Data.SqlClient;

namespace Flash.ConfigureBeforeLaunching;
internal class ConfigureFlashCardDatabase
{
    internal static void CreateFlashCardDatabase()
    {        
        try
        {            
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string checkDatabaseQuery =
                    @"SELECT COUNT(*) 
                    FROM sys.databases 
                    WHERE name = 'DataBaseFlashCard'";

                using (SqlCommand checkDatabaseQueryCommand = new SqlCommand(checkDatabaseQuery, connection))
                {
                    int databaseCount = Convert.ToInt32(checkDatabaseQueryCommand.ExecuteScalar());
                    if (databaseCount == 0)
                    {
                        string createDatabaseQuery =
                            @"CREATE DATABASE DataBaseFlashCard";

                        using (SqlCommand createDatabaseCommand = new SqlCommand(createDatabaseQuery, connection))
                        {
                            createDatabaseCommand.ExecuteNonQuery();
                            Console.WriteLine("Database 'DataBaseFlashCard' created successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Database 'DataBaseFlashCard' created already.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
