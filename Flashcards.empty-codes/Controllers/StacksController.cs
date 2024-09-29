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

    public void InsertStack(StackDTO stack)
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

    public void UpdateStack(StackDTO stack)
    {
        using var conn = new SqlConnection(database.connectionString);
        string updateQuery = "UPDATE Stacks SET StackName = @StackName WHERE StackId = @StackId\"";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(updateQuery, conn);
            cmd.Parameters.AddWithValue("@StackId", stack.StackId);
            cmd.Parameters.AddWithValue("@StackName", stack.StackName);

            int result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                AnsiConsole.MarkupLine($"[yellow]No stack found with the provided Id: {stack.StackId}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Stack Name with Id: {stack.StackId} successfully updated.[/]");
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to change the stack name\n - Details: {e.Message}[/]");
        }
    }

    public void DeleteStack(StackDTO stack)
    {
        using var conn = new SqlConnection(database.connectionString);
        string deleteQuery = "DELETE FROM Stacks WHERE StackId = @StackId";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(deleteQuery, conn);

            int result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                AnsiConsole.MarkupLine($"[yellow]No stack found with the provided Id: {stack.StackId}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Stack with Id: {stack.StackId} successfully deleted.[/]");
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to delete your stack\n - Details: {e.Message}[/]");
        }
    }
}
