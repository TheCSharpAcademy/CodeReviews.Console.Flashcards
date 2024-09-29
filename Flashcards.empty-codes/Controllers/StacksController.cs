using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Models;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flashcards.empty_codes.Controllers;

internal class StacksController
{
    private readonly Database database;

    public StacksController(Database db)
    {
        database = db;
    }

    public int CheckIfStackExists(StackDTO  stack)
    {
        int exists = 0;
        using var conn = new SqlConnection(database.connectionString);
        string checkQuery = "SELECT COUNT(*) FROM Stacks WHERE StackName = @StackName";
        try
        {
            conn.Open();

            using var checkCmd = new SqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@StackName", stack.StackName);
            exists = (int)checkCmd.ExecuteScalar();
        }
        catch (SqlException e)
        {
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to check if stack exists\n - Details: {e.Message}[/]");
            }
        }
        return exists;
    }

    public void InsertStack(StackDTO stack)
    {
        int exists = CheckIfStackExists(stack);
        if (exists > 0)
        {
            AnsiConsole.MarkupLine($"[red]Error: The stack name '{stack.StackName}' already exists![/]");
            return;
        }
        else
        {
            using var conn = new SqlConnection(database.connectionString);
            string insertQuery = "INSERT INTO Stacks(StackName) VALUES (@StackName)";

            try
            {
                conn.Open();
                using var cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@StackName", stack.StackName);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                {
                    AnsiConsole.MarkupLine($"[red]Error occurred while trying to insert your stack\n - Details: {e.Message}[/]");
                }
            }
        }
    }
    public List<StackDTO> ViewAllStacks()
    {
        var stacks = new List<StackDTO>();
        using var conn = new SqlConnection(database.connectionString);
        string readQuery = "SELECT * FROM Stacks";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(readQuery, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                StackDTO stack = new StackDTO
                {
                    StackId = reader.GetInt32(0),
                    StackName = reader.GetString(1)
                };
                stacks.Add(stack);
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to access your stacks\n - Details: {e.Message}[/]");
        }
        return stacks;
    }

    public void UpdateStack(StackDTO stack, string newStackName)
    {
        int exists = CheckIfStackExists(stack);
        if (exists == 0)
        {
            AnsiConsole.MarkupLine($"[yellow]No stack found with the provided name: {stack.StackName}[/]");
            return;
        }
        else
        {
            using var conn = new SqlConnection(database.connectionString);
            string updateQuery = "UPDATE Stacks SET StackName = @NewStackName WHERE StackName = @OldStackName";

            try
            {
                conn.Open();
                using var cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@OldStackName", stack.StackName);
                cmd.Parameters.AddWithValue("@NewStackName", newStackName);

                cmd.ExecuteNonQuery();
                AnsiConsole.MarkupLine($"[green]Stack Name successfully updated from '{stack.StackName}' to '{newStackName}'.[/]");
            }
            catch (SqlException e)
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to change the stack name\n - Details: {e.Message}[/]");
            }
        }
    }

    public void DeleteStack(StackDTO stack)
    {
        int exists = CheckIfStackExists(stack);
        if (exists == 0)
        {
            AnsiConsole.MarkupLine($"[yellow]No stack found with the provided name: {stack.StackName}[/]");
            return;
        }
        else
        {
            using var conn = new SqlConnection(database.connectionString);
            string deleteQuery = "DELETE FROM Stacks WHERE StackName = @StackName";

            try
            {
                conn.Open();
                using var cmd = new SqlCommand(deleteQuery, conn);

                int result = cmd.ExecuteNonQuery();

                AnsiConsole.MarkupLine($"[green]Stack with Name: {stack.StackName} successfully deleted.[/]");
            }
            catch (SqlException e)
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to delete your stack\n - Details: {e.Message}[/]");
            }
        }
    }
}
