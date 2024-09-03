using FlashcardsLibrary;
using Spectre.Console;

namespace Flashcards;
public class Study
{
    public Stack SelectedStack {  get; set; }

    public void Run()
    {
        StackMenuHandler.ShowStacks();
        string stackName = StackMenuHandler.GetStackNameFromUser(new ExistingModelValidator<string, Stack>
        { errorMsg = "Stack Name must exist", GetModel = StackController.GetStackByName });
        if (CancelSetup.IsCancelled(stackName)) return;

        SelectedStack = StackController.GetStackByName(stackName);

        List<SessionQuestion> sessionQuestions = new List<SessionQuestion>();

        MenuPresentation.PresentMenu($"[blue]Studying - Current Stack: {SelectedStack.Name}[/]");
        var flashcards = FlashcardController.GetFlashcardsByStackId(SelectedStack.StackId).OrderBy(flashcard => Guid.NewGuid()).ToList();
        if (flashcards.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]The Stack {SelectedStack.Name} doesn't have flashcards associated with it.[/]");
            Prompter.PressKeyToContinuePrompt();
            return;
        }

        int sessionId = CreateStudySession();

        if (!ShowQuestions(flashcards, sessionQuestions, sessionId))
        {
            StudySessionController.DeleteStudySession(new StudySession { SessionId = sessionId });
            return;
        }

        CreateQuestionsForSession(sessionQuestions, sessionId);
        AnsiConsole.MarkupLine($"[green]Score: {StudySessionController.GetStudySessionById(sessionId).Score}[/]");
        Prompter.PressKeyToContinuePrompt();
    }

    private int CreateStudySession()
    {
        return StudySessionController.InsertStudySession(new StudySession { StackId = SelectedStack.StackId, SessionDate = DateTime.Now });
    }

    private bool ShowQuestions(List<Flashcard> flashcards, List<SessionQuestion> sessionQuestions, int sessionId)
    {
        foreach (var flashcard in flashcards)
        {
            AnsiConsole.WriteLine($"Question: {flashcard.Question}");
            AnsiConsole.WriteLine();
            string userAnswer = FlashcardMenuHandler.GetAnswerFromUser();
            if (CancelSetup.IsCancelled(userAnswer)) return false;

            bool isCorrect = userAnswer.Trim().ToLower() == flashcard.Answer.Trim().ToLower();
            if (isCorrect )
            {
                AnsiConsole.MarkupLine($"[green]Correct![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Wrong! The correct answer is: {flashcard.Answer}[/]");
            }
            Prompter.PressKeyToContinuePrompt();

            sessionQuestions.Add(new SessionQuestion
            {
                SessionId = sessionId,
                QuestionText = flashcard.Question,
                AnswerText = flashcard.Answer,
                UserAnswer = userAnswer,
                IsCorrect = isCorrect,
            });
            AnsiConsole.Clear();
            MenuPresentation.PresentMenu($"[blue]Studying - Current Stack: {SelectedStack.Name}[/]");
        }
        return true;
    }

    private void CreateQuestionsForSession(List<SessionQuestion> sessionQuestions, int sessionId)
    {
        foreach(var question in sessionQuestions)
        {
            SessionQuestionController.InsertSessionQuestion(question);
        }
    }
}