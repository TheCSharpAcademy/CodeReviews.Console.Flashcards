using System.Configuration;
using Dapper;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Flashcards.Controllers;

internal class CardStackController
{
    private string stackName;
    private string connectionString;

    internal CardStackController()
    {
        connectionString = ConfigurationManager.ConnectionStrings["dbString2"].ConnectionString;
    }

    internal void AddStack(string stackName)
    {
        var sql = "INSERT INTO Stacks (Name) VALUES (@Name)";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Execute(sql, new { Name = stackName });
                AnsiConsole.MarkupLine($"[green]Stack {stackName} added successfully![/]");
            }
            catch (SqlException e)
            {
                AnsiConsole.MarkupLine($"[red]Error adding a stack:[/] {e.Message}");
                Console.ReadKey();
            }
        }
    }

    internal List<CardStack> GetAllStacks()
    {
        List<CardStack> stacks = new();
        var sql = "SELECT * FROM Stacks ORDER BY Id";
        using (var connection = new SqlConnection(connectionString))
        {
            stacks = connection.Query<CardStack>(sql).ToList();
        }
        return stacks;
    }

    internal Dictionary<string, int> GetStackNameToIdMap()
    {
        List<CardStack> stacks = GetAllStacks();
        return stacks.ToDictionary(stack => stack.Name, stack => stack.Id);
    }

    internal void DeleteStack(int id)
    {
        var sql = "DELETE FROM Stacks WHERE Id = @Id";

        if (StackExists(id))
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, new { Id = id });
            }
        }
        else
        {
            Console.WriteLine("Stack with this ID does not exist!");
        }
    }

    internal void EditStack(int id, string name)
    {
        var sql = "UPDATE Stacks SET Name = @Name WHERE Id = @Id";
        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Execute(sql, new { Name = name, Id = id });
            }
            catch (SqlException e)
            {
                AnsiConsole.MarkupLine($"[red]Error editing a stack:[/] {e.Message}");
                Console.ReadKey();
            }
        }
    }

    internal bool StackExists(int id)
    {
        bool exists;
        var sql = "SELECT COUNT(*) FROM Stacks WHERE Id = @Id";
        using (var connection = new SqlConnection(connectionString))
        {
            exists = connection.ExecuteScalar<int>(sql, new { Id = id }) > 0;
        }
        return exists;
    }

    internal string GetStackNameById(int id)
    {
        string name;
        var sql = "SELECT Name FROM Stacks WHERE Id = @Id";
        using (var connection = new SqlConnection(connectionString))
        {
            name = connection.ExecuteScalar<string>(sql, new { Id = id });
        }
        return name;
    }
}