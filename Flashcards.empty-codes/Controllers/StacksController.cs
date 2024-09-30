using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Models;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flashcards.empty_codes.Controllers;

internal class StacksController
{
    public StackDTO GetStackById(int id)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        string query = "SELECT * FROM Stacks WHERE StackId = @StackId";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StackId", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new StackDTO
                {
                    StackId = reader.GetInt32(0),
                    StackName = reader.GetString(1)
                };
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to fetch the stack\n - Details: {e.Message}[/]");
        }
        return null;
    }

    public int CheckIfStackExists(StackDTO stack)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        string checkQuery = "SELECT StackId FROM Stacks WHERE StackName = @StackName";
        int exists = 0;

        try
        {
            conn.Open();

            using var checkCmd = new SqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@StackName", stack.StackName);

            object result = checkCmd.ExecuteScalar();

            if (result != null)
            {
                stack.StackId = (int)result;
                exists = 1;
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to check if stack exists\n - Details: {e.Message}[/]");
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
            using var conn = new SqlConnection(Database.ConnectionString);
            string insertQuery = "INSERT INTO Stacks(StackName) VALUES (@StackName)";

            try
            {
                conn.Open();
                using var cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@StackName", stack.StackName);
                int result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]Stack could not be added.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[green]Stack successfully added.[/]");
                }
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
        using var conn = new SqlConnection(Database.ConnectionString);
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
            using var conn = new SqlConnection(Database.ConnectionString);
            string updateQuery = "UPDATE Stacks SET StackName = @NewStackName WHERE StackName = @OldStackName";

            try
            {
                conn.Open();
                using var cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@OldStackName", stack.StackName);
                cmd.Parameters.AddWithValue("@NewStackName", newStackName);

                int result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    AnsiConsole.MarkupLine($"[green]Stack Name could not be updated.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[green]Stack Name successfully updated from '{stack.StackName}' to '{newStackName}'.[/]");
                }
                
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
            using var conn = new SqlConnection(Database.ConnectionString);
            string deleteQuery = "DELETE FROM Stacks WHERE StackName = @StackName";

            try
            {
                conn.Open();
                using var cmd = new SqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@StackName", stack.StackName);
                int result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]Stack could not be deleted.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[green]Stack with Name: {stack.StackName} successfully deleted.[/]");
                }
            }
            catch (SqlException e)
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to delete your stack\n - Details: {e.Message}[/]");
            }
        }
    }
}