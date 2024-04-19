using DbHelpers.HopelessCoding;
using HelperMethods.HopelessCoding;
using Spectre.Console;
using System.Data.SqlClient;

namespace Stacks.HopelessCoding;

internal class StackCommands
{
    internal static void CreateNewStack()
    {
        string createStackQuery = "INSERT INTO Stacks " +
                                  "VALUES (@StackName);";

        Console.WriteLine("Write a name of the new stack:");
        var stackName = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(DatabaseHelpers.connectionString))
        {
            try
            {
                connection.Open();

                SqlCommand createStackCommand = new SqlCommand(createStackQuery, connection);
                createStackCommand.Parameters.AddWithValue("@StackName", stackName);
                createStackCommand.ExecuteNonQuery();

                AnsiConsole.Write(new Markup("[green]\nStack created successfully.[/]\n\n"));
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"[red]$An error occurred: {ex.Message}[/]"));
            }
        }
        Helpers.WaitForUserInput();
    }

    internal static void DeleteStack(string stackName)
    {
        string deleteStackQuery = "DELETE FROM Stacks " +
                                  "WHERE StackName = @StackName;";

        using (SqlConnection connection = new SqlConnection(DatabaseHelpers.connectionString))
        {
            try
            {
                connection.Open();

                SqlCommand createStackCommand = new SqlCommand(deleteStackQuery, connection);
                createStackCommand.Parameters.AddWithValue("@StackName", stackName);
                createStackCommand.ExecuteNonQuery();

                AnsiConsole.Write(new Markup("[green]Stack deleted successfully.[/]\n\n"));
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"[red]$An error occurred: {ex.Message}[/]"));
            }
        }
        Helpers.WaitForUserInput();
    }

    internal static void EditStack(string stackName)
    {
        string editStackQuery = "UPDATE Stacks " +
                                "SET StackName = @newStackName " +
                                "WHERE StackName = @StackName;";

        Console.Write("\nInsert a new stack name: ");
        var newStackName = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(DatabaseHelpers.connectionString))
        {
            try
            {
                connection.Open();

                SqlCommand createStackCommand = new SqlCommand(editStackQuery, connection);
                createStackCommand.Parameters.AddWithValue("@StackName", stackName);
                createStackCommand.Parameters.AddWithValue("@newStackName", newStackName);
                createStackCommand.ExecuteNonQuery();

                AnsiConsole.Write(new Markup("[green]\nStack name changed successfully.[/]\n\n"));
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"[red]$An error occurred: {ex.Message}[/]"));
            }
        }
        Helpers.WaitForUserInput();
    }
}