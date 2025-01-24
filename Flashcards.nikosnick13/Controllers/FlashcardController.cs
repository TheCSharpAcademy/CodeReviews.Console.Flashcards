using Dapper;
using Flashcards.nikosnick13.DTOs;
using Flashcards.nikosnick13.Models;
using Microsoft.Data.SqlClient;
using static System.Console;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flashcards.nikosnick13.UI;

namespace Flashcards.nikosnick13.Controllers;

internal class FlashcardController
{
    private string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    public void InsertFlashcart(BasicFlashcardDTO flashcards)
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"INSERT INTO Flashcards(Stack_Id, Question, Answer) VALUES(@stackId, @question, @answer)";

            conn.Execute(query, new {
                stackId = flashcards.StackId, 
                question = flashcards.Question, 
                answer = flashcards.Answer
            });


            AnsiConsole.MarkupLine($"\n[blue]A flashcard with the question '{flashcards.Question}' was added to Stack ID {flashcards.StackId}![/]");

            AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to continue...").AllowEmpty());
        }
        catch(Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");

        }
    }

    public List<DetailFlashcardDTO> ViewAllFlashcards()
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"SELECT Id, Question, Answer, Stack_Id AS StackId FROM Flashcards";
            
            // Χρήση Dapper για την εκτέλεση του query
            var flashcards = conn.Query<DetailFlashcardDTO>(query).ToList();

            WriteLine(flashcards);

            if (!flashcards.Any())
            {
                WriteLine("\n\nNo flashcards found.\n\n");
            }
            ReadKey();
            TableVisualisation.DisplayFlashcards(flashcards);
            ReadKey();
            return flashcards;
        }
        catch (Exception ex)
        {
            WriteLine("Error: " + ex.Message);
            return new List<DetailFlashcardDTO>(); // Επιστροφή κενής λίστας σε περίπτωση σφάλματος
        }
    }

    public Flashcard GetById(int id) {

        try {
            using var conn = new SqlConnection(connectionString);

            conn.Open();

            string query = "SELECT Id FROM Flashcards WHERE Id = @id";

            var result = conn.QueryFirstOrDefault<Flashcard>(query, new {id});

            return result;
        }
        catch(Exception ex)
        {
            WriteLine($"Error fetching Stack with ID {id}: {ex.Message}");
            return null;
        }
    } 

    



}
