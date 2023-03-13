using Microsoft.Data.SqlClient;
using System;

namespace DbCommandsLibrary; 

public class Deletion 
{
    string connectionString, cardsTableName, stacksTableName;
    Returning returning;

    public Deletion(string connectionString, string cardsTableName, string stacksTableName)
    {
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.connectionString = connectionString;

        returning = new(connectionString, cardsTableName, stacksTableName);
    }
    public bool StackByIndex(int viewId)
    {
        int id = returning.IdFromViewId(this.stacksTableName,viewId);

        if(!ByIndex(this.stacksTableName, id)) return false;
        else 
        {
            if (UpdateViewIdsAfterDeletion(this.stacksTableName, viewId)) return true;
            else throw new Exception("Corrupted table after deletion");
        }
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

    private bool UpdateViewIdsAfterDeletion(string tableName, int deletedViewId)
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
                    if (command.ExecuteNonQuery() > 0) return true;
                }
            }
            return false;
        }
        catch (SqlException) { return false; }
    }
}
