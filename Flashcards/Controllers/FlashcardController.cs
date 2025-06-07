using Flashcards.Data;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Controllers;

public class FlashcardController
{
    internal static void AddFlashcard()
    {
        Flashcard flashcard = new Flashcard();
        DataConnection dataConnection = new DataConnection();

        flashcard.CategoryId = CategoryController.ChooseCategory("Choose Category");
        flashcard.Question = GetQuestion();
        flashcard.Answer = GetAnswer();

        while (string.IsNullOrWhiteSpace(flashcard.Question))
        {
            flashcard.Question = AnsiConsole.Ask<string>("Name cannot be empty. Insert Question:");
        }

        dataConnection.InsertFlashcard(flashcard);
    }


    public static void ViewAllFlashcards(IEnumerable<Flashcard> flashcards)
    {
        var table = new Table();
        table.AddColumn(new TableColumn("Category"));
        table.AddColumn(new TableColumn("Question"));
        table.AddColumn(new TableColumn("Answer"));

        foreach (var flashcard in flashcards)
        {
            table.AddRow(flashcard.CategoryId.ToString(), flashcard.Question, flashcard.Answer);
        }

        AnsiConsole.Write(table);
    }

    internal static void UpdateFlashcard()
    {
        var categoryId = CategoryController.ChooseCategory("Choose category:");
        var flashcardId = ChooseFlashcard("Choose flashcard to update: ", categoryId);

        var propertiesToUpdate = new Dictionary<string, object>();
        if (AnsiConsole.Confirm("Would you like to update the flashcard question?"))
        {
            var question = GetQuestion();
            propertiesToUpdate.Add("Question", question);
        }

        if (AnsiConsole.Confirm("Would you like to update the flashcard answer?"))
        {
            var answer = GetAnswer();
            propertiesToUpdate.Add("Answer", answer);
        }

        if (AnsiConsole.Confirm("Would you like to update the flashcard category?"))
        {
            var category = CategoryController.ChooseCategory("Choose category:");
            propertiesToUpdate.Add("CategoryId", category);
        }

        DataConnection dataConnection = new DataConnection();
        dataConnection.UpdateFlashcard(flashcardId, propertiesToUpdate);
    }

    internal static void DeleteFlashcard()
    {
        var categoryId = CategoryController.ChooseCategory("Which category?");
        var flashcard = ChooseFlashcard("Which flashcard do you want to delete?", categoryId);

        if (!AnsiConsole.Confirm("Are you sure you want to delete this flashcard?"))
        {
            return;
        }

        DataConnection dataConnection = new DataConnection();
        dataConnection.DeleteFlashcard(flashcard);
    }

    internal static int ChooseFlashcard(string message, int categoryId)
    {
        DataConnection dataConnection = new DataConnection();
        var flashcards = dataConnection.GetFlashcardByCategory(categoryId);
        var flashcardArray = flashcards.Select(f => f.Question).ToArray();
        var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(message)
            .AddChoices(flashcardArray));
        var flashcardId = flashcards.FirstOrDefault(f => f.Question == option).Id;

        return flashcardId;
    }

    private static string GetQuestion()
    {
        var question = AnsiConsole.Ask<string>("Insert Question:");

        while (string.IsNullOrEmpty(question))
        {
            question = AnsiConsole.Ask<string>("Question can't be empty. Try again.");
        }

        return question;
    }

    private static string GetAnswer()
    {
        var answer = AnsiConsole.Ask<string>("Insert Answer:");

        while (string.IsNullOrEmpty(answer))
        {
            answer = AnsiConsole.Ask<string>("Answer can't be empty. Try again.");
        }

        return answer;
    }
}