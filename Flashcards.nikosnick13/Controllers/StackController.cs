using Dapper;
using Flashcards.nikosnick13.Models;
using Microsoft.Data.SqlClient;
using System;
using static System.Console;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flashcards.nikosnick13.UI;
using Spectre.Console;
using Flashcards.nikosnick13.DTOs;

namespace Flashcards.nikosnick13.Controllers;

internal class StackController
{
    static string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    public void InsertStack(Stack stack)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        var checkQuery = @"SELECT COUNT(1) FROM Stacks WHERE Name = @name";
        var exists = conn.ExecuteScalar<int>(checkQuery, new { name = stack.Name });

        if (exists > 0)
        {
            AnsiConsole.MarkupLine("\n[red]A stack with this name already exists![/]");
            AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to continue...").AllowEmpty());
            return;
        }

        string query = @"INSERT INTO Stacks (Name) VALUES (@name)";
        conn.Execute(query, new { name = stack.Name });
        AnsiConsole.MarkupLine($"\n[blue]A stack with this name {stack.Name} add to list![/]");
        AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to continue...").AllowEmpty());
    }

    public List<DetailStackDTO> ViewAllStacks() 
    {
        Clear();
        List<DetailStackDTO> recordsList = new List<DetailStackDTO>();

        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"SELECT * FROM Stacks";

            using var command = new SqlCommand(query, conn);

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    recordsList.Add(new DetailStackDTO
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Flashcard = null,  //  Αν υπάρχουν Flashcards, θα χρειαστεί πρόσθετο query
                        StudySession = null  //  Αν υπάρχουν StudySession, θα χρειαστεί πρόσθετο query
                    });
                }
            }
            else WriteLine("\n\nNo rows fount\n\n");
        }
        catch (Exception ex)
        {
            WriteLine("Error: " + ex.Message);
        }

        TableVisualisation.ShowTable(recordsList);

        return recordsList;

    }


    public Stack? GetById(int id) 
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"SELECT * FROM Stacks WHERE Id = @id ";

            var result = conn.QueryFirstOrDefault<Stack>(query, new {id});
            return result;
        }
        catch(Exception ex)
        {
            WriteLine($"Error fetching Stack with ID {id}: {ex.Message}");
            return null;
        }
    
    }

    public void DeleteStackById(int id) 
    {
        try 
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"DELETE FROM Stacks WHERE Id = @id";

            var rowsAffected = conn.Execute(query, new { id });

            if (rowsAffected > 0)
            {
                WriteLine($"Stack with ID {id} was deleted successfully.");
                AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to continue...").AllowEmpty());
            }
            else
            {
                WriteLine($"\n\nNo record found with Id {id}. Nothing was deleted preess any key to return..\n\n");
                ReadKey();
            }
        } 
        catch(Exception ex)
        {
            WriteLine("Error " + ex.Message);
        }
    }


    public void EditStackById(BasicStackDTO basicStackDTO) 
    {
        try 
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"UPDATE Stacks SET Name = @name WHERE Id = @id";

            conn.Execute(query, new
            {
                name = basicStackDTO.Name,
                id = basicStackDTO.Id
            });
        } 
        catch(Exception ex) 
        {
            WriteLine("Error " + ex.Message);
        }
    }

    public DetailStackDTO ViewStackById(int id)
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"SELECT Id, Name FROM Stacks WHERE Id = @id";

            // Επιστρέφει DetailStackDTO
            return conn.QueryFirstOrDefault<DetailStackDTO>(query, new { id });
        }
        catch (Exception ex)
        {
            WriteLine("Error: " + ex.Message);
            return null;
        }
    }


    // ViewAllStack: this method return list.Interact with flashcardsUI
    public List<DetailStackDTO> ViewAllStack()
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"SELECT Id, Name FROM Stacks";

            var stacks = conn.Query<DetailStackDTO>(query).ToList();

            return stacks;
        }
        catch (Exception ex)
        {
            WriteLine($"Error fetching stacks: {ex.Message}");
            return new List<DetailStackDTO>();  
        }
    }

}
