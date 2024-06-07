using Flashcards.Models;
using Spectre.Console;

namespace Flashcards;

internal class StudyGameEngine
{
    StudySession GameStudySession;
    FlashcardsDbContext DbContext;
    Stack SelectedStack;
    IEnumerable<FlashcardDto>? Flashcards;
    public StudyGameEngine(FlashcardsDbContext dbContext, Stack stack)
    {
        DbContext = dbContext;
        GameStudySession = new StudySession();
        SelectedStack = stack;
        Flashcards = DbContext.GetAllFlashcardDtoByStackId(SelectedStack.Id);
        GameStudySession.TotalQuestions = Flashcards.Count();
        GameStudySession.StackId = stack.Id;
    }

    internal void StartGame()
    {
        if(GameStudySession.TotalQuestions == 0)
        {
            AnsiConsole.Markup("There is no Flashcards in the Stack. Please select another Stack\n");
            VisualizationEngine.DisplayContinueMessage();
            return;
        }
        foreach (var flashcard in Flashcards)
        {
            if(!RunQuestion(flashcard))
            {
                VisualizationEngine.ShowResultMessage(1, "You quit the Study Session");
                return;
            }
        }
        EndGame();
    }

    private void EndGame()
    {
        GameStudySession.StudyDate = DateTime.Now.AddDays(30);
        DbContext.AddStudySession(GameStudySession);
        AnsiConsole.Markup($"Your final score is [green]{GameStudySession.Score}[/].\n");
        VisualizationEngine.ShowResultMessage(1, "The Study Session is added to database");
    }

    internal bool RunQuestion(FlashcardDto flashcard)
    {
        AnsiConsole.Clear();
        ShowQuestion(flashcard, false);
        string? answer = AnsiConsole.Ask<string>($"Type your [green]Answer[/] and Press [blue]Enter[/] or Enter [maroon]0[/] to [maroon]quit the Study Session[/]: ");
        if(answer.Trim().ToLower() == "0")
        {
            return false;
        }
        AnsiConsole.Clear();
        if (answer.Trim().ToLower() == flashcard.Back.ToLower())
        {
            GameStudySession.Score++;
            ShowQuestion(flashcard, true);
            AnsiConsole.Markup("[green]Very Good, Your answer is correct[/]\n");
        }
        else
        {
            ShowQuestion(flashcard, true);
            AnsiConsole.Markup("[maroon]Sorry, Your answer is incorrect[/]\n");

        }
        VisualizationEngine.DisplayContinueMessage();
        return true;
    }

    private void ShowQuestion(FlashcardDto flashcard, bool showAnswer)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var table = new Table();
        table.Border = TableBorder.Square;
        table.ShowRowSeparators = true;
        table.Title = new TableTitle($"Studying [blue]{SelectedStack.StackName}[/] Stack");
        table.AddColumn("Flashcard #");
        table.AddColumn("Current Score");
        table.AddColumn("Front");
        if (showAnswer)
        {
            table.AddColumn("Back");
            table.AddRow(flashcard.Key.ToString(), GameStudySession.Score.ToString(), flashcard.Front, flashcard.Back);
        }
        else
        {
            table.AddRow(flashcard.Key.ToString(), GameStudySession.Score.ToString(), flashcard.Front);
        }
        AnsiConsole.Write(table);
    }

}