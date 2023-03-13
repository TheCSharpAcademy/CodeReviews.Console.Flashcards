using Microsoft.Data.SqlClient;
using ObjectsLibrary;

namespace DbCommandsLibrary;

public class Updating
{
    string connectionString, cardsTableName, stacksTableName;

    public Updating(string connectionString, string cardsTableName, string stacksTableName)
    {
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.connectionString = connectionString;

    }

    public bool StackByIndex(int index, Stack updatedStack)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                connection.Open();

                String sql =
                $"UPDATE {this.stacksTableName} SET Name='{updatedStack.Name}' WHERE Id={index}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if(command.ExecuteNonQuery() > 0) return true;
                }
            }
            return false;
        }
        catch (SqlException) { return false; }
    }

    public bool CardByIndex(int index, Card updatedCard) 
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"UPDATE {this.cardsTableName} SET " +
                $"Prompt='{updatedCard.Prompt}', Answer='{updatedCard.Answer}' " +
                $"WHERE Id={index}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (command.ExecuteNonQuery() > 0) return true;
                }
            }
            return false;
        }
        catch (SqlException) { return false; }
    }
}
