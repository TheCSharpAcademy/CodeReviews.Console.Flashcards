using System.Configuration;
using ConsoleTableExt;
using Microsoft.Data.SqlClient;

namespace Flashcards.jkjones98;

internal class StackController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    internal void InsertStackDb(Stack stack)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = 
            $@"INSERT INTO Stacks (StackName) VALUES ('{stack.StackName}')";
        tableCmd.ExecuteNonQuery();
    }

    internal void ViewStackDb()
    {
        List<Stack> stackTable = new List<Stack>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = "SELECT * FROM Stacks";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                stackTable.Add(new Stack
                    {
                        StackId =  reader.GetInt32(0),
                        StackName = reader.GetString(1)
                    });
            }
        }
        else
            Console.WriteLine("\nNo rows found");

        ShowTable.CreateStackTable(stackTable);
    }

    internal void ViewStackDb(int stackId)
    {
        List<Stack> stackTable = new List<Stack>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM Stacks WHERE StackId={stackId}";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                stackTable.Add(new Stack
                    {
                        StackId =  reader.GetInt32(0),
                        StackName = reader.GetString(1)
                    });
            }
        }
        else
            Console.WriteLine("\nNo rows found");

        ShowTable.CreateStackTable(stackTable);
    }

    internal void ChangeStackDb(int stackId, string newStackName)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $@"UPDATE Stacks
                SET StackName = {newStackName}
                WHERE StackId={stackId}";
        tableCmd.ExecuteNonQuery();
    }

    internal void DeleteStackDb(int stackId)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"DELETE FROM Stacks WHERE StackId={stackId}";
        tableCmd.ExecuteNonQuery();
    }

    internal bool CheckNameExists(string stackName)
    {
        bool checkedName = false;

        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = 
            $@"SELECT * FROM Stacks WHERE CONVERT (VARCHAR, StackName)='{stackName}'";
        using var reader = tableCmd.ExecuteReader();
        Stack stackCheck = new();
        if(reader.HasRows)
        {
            reader.Read();
            stackCheck.StackId = reader.GetInt32(0);
            stackCheck.StackName = reader.GetString(1);
            if(stackCheck.StackName.ToLower() == stackName.ToLower()) checkedName = true;
        }
        
        return checkedName;
    }
}