using Flashcards.Data;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Controllers;

public class StudySessionController
{
    internal static void CreateStudySession()
    {
        var id = CategoryController.ChooseCategory("Select Category");
        DataConnection dataConnection = new DataConnection();
        var flashcards = dataConnection.GetFlashcardByCategory(id);

        StudySession studySession = new StudySession();
        studySession.Questions = flashcards.Count();
        studySession.CategoryId = id;
        studySession.Date = DateTime.Now;

        var correctAnswers = 0;

        foreach (var flashcard in flashcards)
        {
            var answer = AnsiConsole.Ask<string>($"{flashcard.Question}: ");

            while (string.IsNullOrEmpty(answer))
            {
                answer = AnsiConsole.Ask<string>($"Answer cannot be empty. {flashcard.Question}: ");
            }

            if (string.Equals(answer.Trim(), flashcard.Answer, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                AnsiConsole.WriteLine("Correct!");
            }
            else
            {
                AnsiConsole.WriteLine($"Incorrect. The answer is {flashcard.Answer}.");
            }
        }

        AnsiConsole.WriteLine($"You got {correctAnswers} out of  {flashcards.Count()} correct!");
        studySession.CorrectAnswers = correctAnswers;
        studySession.Time = DateTime.Now - studySession.Date;

        dataConnection.InsertStudySession(studySession);
    }

    internal static void ViewStudyHistory()
    {
        DataConnection dataConnection = new DataConnection();
        var sessions = dataConnection.GetStudySessions();
        var table = new Table();

        table.AddColumn(new TableColumn("Date"));
        table.AddColumn(new TableColumn("Category"));
        table.AddColumn(new TableColumn("Result"));
        table.AddColumn(new TableColumn("Percentage"));
        table.AddColumn(new TableColumn("Duration"));

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Date.ToShortDateString(),
                session.CategoryName,
                $"{session.CorrectAnswers} out of {session.Questions}",
                $"{session.Percentage}%",
                session.Time.ToString());
        }

        AnsiConsole.Write(table);
    }
}