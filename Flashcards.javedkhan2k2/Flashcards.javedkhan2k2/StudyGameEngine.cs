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
            AnsiConsole.Markup("There is no Flashcards in the Stack. Please select another Stack\n".ToUpper());
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
        AnsiConsole.Markup($"Your final score is [green]{GameStudySession.Score}[/].\n".ToUpper());
        VisualizationEngine.ShowResultMessage(1, "The Study Session is added to database");
    }

    internal bool RunQuestion(FlashcardDto flashcard)
    {
        AnsiConsole.Clear();
        ShowQuestion(flashcard, false);
        string? answer = AnsiConsole.Ask<string>($"Type your [green]Answer[/] and Press [blue]Enter[/] or Enter [maroon]0[/] to [maroon]quit the Study Session[/]: ".ToUpper());
        
        if(answer.Trim().ToLower() == "0") return false;
        
        AnsiConsole.Clear();
        if (answer.Trim().ToLower() == flashcard.Back.Trim().ToLower())
        {
            GameStudySession.Score++;
            
            ShowQuestion(flashcard, true);
            AnsiConsole.Markup($"[green]Very Good,[/] Your answer [lightgoldenrod1]{answer}[/] is correct\n".ToUpper());
        }
        else
        {
            ShowQuestion(flashcard, true);
            AnsiConsole.Markup($"[maroon]Sorry[/], Your answer [lightgoldenrod1]{answer}[/] is incorrect\n".ToUpper());

        }
        VisualizationEngine.DisplayContinueMessage();
        return true;
    }

    private void ShowQuestion(FlashcardDto flashcard, bool showAnswer)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var table = VisualizationEngine.CreateTable($"Studying [blue]{SelectedStack.StackName}[/] Stack");
        table.AddColumn("Flashcard #");
        table.AddColumn("Current Score");
        table.AddColumn("Front");
        if (showAnswer)
        {
            table.AddColumn("Back");
            table.AddRow(flashcard.Key.ToString(), GameStudySession.Score.ToString(), $"[green]{flashcard.Front}[/]", $"[lightgoldenrod1]{flashcard.Back}[/]");
        }
        else
        {
            table.AddRow(flashcard.Key.ToString(), GameStudySession.Score.ToString(), $"[green]{flashcard.Front}[/]");
        }
        AnsiConsole.Write(table);
    }

}