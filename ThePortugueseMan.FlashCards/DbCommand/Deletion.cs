using Microsoft.Data.SqlClient;

namespace DbCommandsLibrary; 

public class Deletion 
{
    string connectionString, cardsTableName, stacksTableName, studySessionsTableName;
    Returning returning;

    public Deletion(string connectionString, string cardsTableName, string stacksTableName, string studySessionsTableName)
    {
        this.connectionString = connectionString;
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.studySessionsTableName = studySessionsTableName;

        returning = new(connectionString, cardsTableName, stacksTableName, studySessionsTableName);
    }
    public bool StackByViewId(int viewId)
    {
        int index = returning.IdFromViewId(this.stacksTableName,viewId);

        if(!ByIndex(this.stacksTableName, index)) return false;
        else 
        {
            UpdateViewIdsAfterDeletion(this.stacksTableName, viewId);
            return true;
        }
    }

    public bool CardByViewId(int viewId)
    {
        int index = returning.IdFromViewId(this.cardsTableName, viewId);

        if (!ByIndex(this.cardsTableName, index)) return false;
        else
        {
            UpdateViewIdsAfterDeletion(this.cardsTableName, viewId);
            return true;
        }
    }

    private bool ByIndex(string tableName, int index)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"DELETE FROM {tableName} WHERE Id={index}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (command.ExecuteNonQuery() > 0) return true;
                }
            }
            return false;
        }
        catch (SqlException) { return false; }
    }

    private void UpdateViewIdsAfterDeletion(string tableName, int deletedViewId)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"UPDATE {tableName} SET ViewId = ViewId-1 WHERE ViewId > {deletedViewId}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (command.ExecuteNonQuery() > 0) return;
                }
            }
            return;
        }
        catch (SqlException) { return; }
    }
}
