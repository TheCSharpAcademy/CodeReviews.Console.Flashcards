using Microsoft.Data.SqlClient;
using ObjectsLibrary;
using SettingsLibrary;

namespace DbCommandsLibrary;

public class Insertion
{
    string connectionString, cardsTableName, stacksTableName;

    public Insertion(string connectionString, string cardsTableName, string stacksTableName)
    {
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.connectionString = connectionString;
    }

    public bool IntoTable(Stack stack)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                connection.Open();

                String sql =
                $"INSERT INTO {this.stacksTableName} (ViewId,Name) " +
                $"VALUES ({stack.ViewId},'{stack.Name}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        return true;
        }
        catch (SqlException) { return false; }
    }

    public bool IntoTable(Card card)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                connection.Open();

                String sql =
                $"INSERT INTO {this.cardsTableName} (Prompt,Answer,StackId) " +
                $"VALUES ('{card.Prompt}','{card.Answer}',{card.StackId})";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if(command.ExecuteNonQuery() > 0) return true;
                }
            }
            return false;
        }
        catch (SqlException) { return false; }
    }
}
