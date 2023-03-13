using Microsoft.Data.SqlClient;

namespace DbCommandsLibrary; 

public class Deletion 
{
    string connectionString, cardsTableName, stacksTableName;

    public Deletion(string connectionString, string cardsTableName, string stacksTableName)
    {
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.connectionString = connectionString;

    }
    public bool StackByIndex(int index)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                connection.Open();

                String sql =
                $"DELETE FROM {this.stacksTableName} WHERE Id={index}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (SqlException) { return false; }
    }

    public bool CardByIndex(int index)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                connection.Open();

                String sql =
                $"DELETE FROM {this.cardsTableName} WHERE Id={index}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (SqlException) { return false; }
    }
}
