using Microsoft.Data.SqlClient;
using SettingsLibrary;

namespace DbCommandsLibrary;

public class Initialization
{
    string cardsTableName, stacksTableName, connectionString;
    int stackNameLimit, cardNameLimit, cardPromptLimit, cardAnswerLimit;

    internal Initialization
        (string connectionString, string cardsTableName, string stacksTableName, int stackNameLimit, int cardNameLimit, int cardPromptLimit, int cardAnswerLimit)
    {
        this.connectionString = connectionString;
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.stackNameLimit = stackNameLimit;
        this.cardNameLimit = cardNameLimit;
        this.cardPromptLimit= cardPromptLimit;
        this.cardAnswerLimit= cardAnswerLimit;
    }
    
    public void AllTables()
    {
        StacksTable();
        CardsTable();
    }

    private void StacksTable()
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                connection.Open();

                String sql =
                $"CREATE TABLE {this.stacksTableName} (" +
                    "Id INTEGER NOT NULL IDENTITY PRIMARY KEY," +
                    $"Name VARCHAR({this.stackNameLimit}) NOT NULL,)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException) { }
    }

    private void CardsTable()
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = this.connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                connection.Open();

                String sql =
                    $"CREATE TABLE {this.cardsTableName} (" +
                        "Id INTEGER NOT NULL IDENTITY PRIMARY KEY," +
                        $"Prompt VARCHAR({this.cardPromptLimit})," +
                        $"Answer VARCHAR({this.cardAnswerLimit})," +
                        $"StackId INTEGER NOT NULL FOREIGN KEY REFERENCES {this.stacksTableName}(Id)" +
                        $"ON DELETE CASCADE ON UPDATE CASCADE)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException) { }
    }
}
