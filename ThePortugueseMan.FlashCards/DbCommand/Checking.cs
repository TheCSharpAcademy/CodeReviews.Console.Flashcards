using Microsoft.Data.SqlClient;
using System;

namespace DbCommandsLibrary;

public class Checking
{
    string connectionString, cardsTableName, stacksTableName;

    public Checking(string connectionString, string cardsTableName, string stacksTableName)
    {
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.connectionString = connectionString;
    }

    public bool StackByIndex(int index)
    {
        return CheckByIndex(index, this.stacksTableName);
    }

    public bool CardByIndex(int index)
    {
        return CheckByIndex(index,this.cardsTableName);
    }

    private bool CheckByIndex(int index, string tableName) 
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {tableName} WHERE Id={index}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (command.ExecuteReader().HasRows) return true;
                }
            }
            return false;
        }
        catch (SqlException) { return false; }
    }

    public bool StackByName(string stackName)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {this.stacksTableName} WHERE Name='{stackName}'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (command.ExecuteReader().HasRows) return true;
                }
            }
            return false;
        }
        catch (SqlException) { return false; }
    }
}
