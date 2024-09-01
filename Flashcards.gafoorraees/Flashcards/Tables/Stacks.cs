using System.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.Models;

namespace Flashcards.Tables;

public class Stacks
{
    private static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");

    public static void InsertStack(string name)
    {
        using var connection = new SqlConnection(connectionString);
        
        string query = "INSERT INTO Stacks (Name) VALUES (@Name)";
        var parameters = new { Name = name };
        
        connection.Execute(query, parameters);
        
    }

    public static List<Stack> GetAllStacks()
    {
        using var connection = new SqlConnection(connectionString);
        
        string query = "SELECT Name FROM Stacks";
            
        return connection.Query<Stack>(query).ToList();
    }

    public static int ReturnStackID(string stackName)
    {
        using var connection = new SqlConnection(connectionString);
        
        string selectQuery = "SELECT ID FROM Stacks WHERE LOWER(Name) = LOWER(@Name)";
        var parameters = new { Name = stackName };
        int stackId = connection.QuerySingleOrDefault<int>(selectQuery, parameters);
        
        return stackId;
        
    }

    public static string ReturnStackName(int stackId)
    {
        using var connection = new SqlConnection(connectionString);
        
        string selectQuery = "SELECT Name FROM Stacks WHERE ID = @StackId";
        var parameters = new { StackId = stackId };
        string stackName = connection.QuerySingle<string>(selectQuery, parameters);

        return stackName;
    }

    public static void DeleteStack()
    {
        while (true)
        {
            Console.Clear();

            StacksUI.DisplayStacks();

            Console.WriteLine("\nPlease enter the name of the stack that you would like to delete, or enter 'M' to return to the main menu. Warning: All flashcards and study sessions linked to the stack will be deleted.\n");
            string stackName = Console.ReadLine().Trim();

            if (stackName == "M" || stackName == "m") 
            {
                Utility.ReturnToMenu();
            }

            int stackId = ReturnStackID(stackName);

            if (stackId == 0)
            {
                Console.WriteLine("\nStack does not exist. Enter a key to try again.\n");
                Console.ReadLine();
                continue;
            }

            using var connection = new SqlConnection(connectionString);
            
            string deleteQuery = "DELETE FROM Stacks WHERE ID = @StackID";
            var parameters = new { StackID = stackId };
            
            connection.Execute(deleteQuery, parameters);

            Console.Clear();

            Console.WriteLine("Stack and its flashcards deleted successfully. Press Enter to return to the main menu.");
            Console.ReadLine();
            break;
        }
    }
}
