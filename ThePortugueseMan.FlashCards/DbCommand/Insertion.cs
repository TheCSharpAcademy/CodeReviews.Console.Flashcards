using Microsoft.Data.SqlClient;
using ObjectsLibrary;

namespace DbCommandsLibrary;

public class Insertion
{
    string connectionString, cardsTableName, stacksTableName, studySessionTableName;

    public Insertion(string connectionString, string cardsTableName, string stacksTableName, string studySessionTableName)
    {
        this.connectionString = connectionString;
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.studySessionTableName = studySessionTableName;
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
                $"INSERT INTO {this.cardsTableName} (ViewId,Prompt,Answer,StackId) " +
                $"VALUES ({card.ViewId},'{card.Prompt}','{card.Answer}',{card.StackId})";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if(command.ExecuteNonQuery() > 0) return true;
                }
            }
            return false;
        }
        catch (SqlException) { return false; }
    }

    public bool IntoTable(StudySession session)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"INSERT INTO {this.studySessionTableName} (Date,Score,RoundsPlayed,StackId) " +
                $"VALUES ('"+ session.Date.ToString("yyyyMMdd HH:mm:ss") +"'," +
                $"{session.Score},{session.RoundsPlayed},{session.StackId})";

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
