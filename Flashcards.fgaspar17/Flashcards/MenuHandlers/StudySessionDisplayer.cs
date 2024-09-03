using FlashcardsLibrary;
using Spectre.Console;

namespace Flashcards;
public class StudySessionDisplayer
{
    public void Display()
    {
        ShowStudySessions();

        string sessionId = GetSessionIdFromUser(new ExistingModelValidator<int, StudySession>
        {
            ErrorMsg = "Study Session Id must exist",
            GetModel = StudySessionController.GetStudySessionById
        });
        if (CancelSetup.IsCancelled(sessionId)) return;
        int sessionIdParsed = Convert.ToInt32(sessionId);

       ShowQuestionsPerSession(sessionIdParsed);
    }

    private string GetSessionIdFromUser(params IValidator[] validators)
    {
        string message = "Introduce an existing Id";

        return Prompter.ValidatedTextPrompt(message, validations: validators);
    }

    private void ShowStudySessions()
    {
        AnsiConsole.Clear();
        MenuPresentation.PresentMenu("[yellow]Study Sessions[/]");

        var studySessions = StudySessionService.GetStudySessions();
        OutputRenderer.ShowTable(studySessions, "Study Sessions");
    }

    private void ShowQuestionsPerSession(int sessionId)
    {
        var questionsPerSession = SessionQuestionService.GetSessionQuestionsBySessionId(sessionId);
        AnsiConsole.Clear();
        OutputRenderer.ShowTable(questionsPerSession, "Questions");
        Prompter.PressKeyToContinuePrompt();
    }
}